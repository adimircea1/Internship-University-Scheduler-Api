using EmailVerification.Library.DataContracts;
using EmailVerification.Library.GrpcServiceInterfaces;
using Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Models.Enums;
using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Internship.AuthorizationAuthentication.Api.Core.Models.Utils;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Internship.UniversityScheduler.Library.DataContracts;
using Internship.UniversityScheduler.Library.GrpcServiceInterfaces;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.GRPC.Grpc_Setups;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IPasswordManager _passwordManager;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenValidator _refreshTokenValidator;
    private readonly IUserAuthenticator _userAuthenticator;
    private readonly IUserService _userService;
    private readonly ILogger<UserAuthenticationService> _logger;
    private readonly IStudentRegisterRequestService _studentRegisterRequestService;
    private readonly IProfessorRegisterRequestService _professorRegisterRequestService;
    private readonly IGrpcClientService _grpcClientService;

    public UserAuthenticationService(
        IPasswordManager passwordManager, 
        IRefreshTokenService refreshTokenService, 
        IRefreshTokenValidator refreshTokenValidator, 
        IUserAuthenticator userAuthenticator, 
        IUserService userService, 
        ILogger<UserAuthenticationService> logger, 
        IStudentRegisterRequestService studentRegisterRequestService, 
        IProfessorRegisterRequestService professorRegisterRequestService, 
        IGrpcClientService grpcClientService)
    {
        _passwordManager = passwordManager;
        _refreshTokenService = refreshTokenService;
        _refreshTokenValidator = refreshTokenValidator;
        _userAuthenticator = userAuthenticator;
        _userService = userService;
        _logger = logger;
        _studentRegisterRequestService = studentRegisterRequestService;
        _professorRegisterRequestService = professorRegisterRequestService;
        _grpcClientService = grpcClientService;
    }

    public async Task<string> RegisterStudentUserAsync(int studentRegisterRequestId)
    {
        var existingRegisterRequest = await _studentRegisterRequestService.GetRegisterRequestByIdAsync(studentRegisterRequestId);

        var userCredentials = CreateCredentials(existingRegisterRequest.FirstName, existingRegisterRequest.LastName, UserType.Student);
        
        var passwordData = _passwordManager.HashPassword(userCredentials.TemporaryPassword);
        
        //Send credentials via email
        await SendUserCredentialsAsync(userCredentials.UserName, existingRegisterRequest.Email, userCredentials.TemporaryPassword);

        //Add a new user
        await CreateUserAsync(userCredentials, passwordData, existingRegisterRequest.Email, UserType.Student);
        
        //Add student in university scheduler api
        var universitySchedulerStudentService = _grpcClientService.GetService<IStudentGrpcService>();
        await universitySchedulerStudentService.AddStudentAsync(new StudentInputDataContract
        {
            FirstName = existingRegisterRequest.FirstName,
            LastName = existingRegisterRequest.LastName,
            Email = userCredentials.UserUniversityEmail,
            BirthdayDate = existingRegisterRequest.Birthdate.ToDateTime(default, DateTimeKind.Utc),
            PhoneNumber = existingRegisterRequest.PhoneNumber,
            StudyYear = 1,
            PersonalEmail = existingRegisterRequest.Email
        });
        
        //Get rid off the current register request
        await _studentRegisterRequestService.DeleteRegisterRequest(existingRegisterRequest);

        _logger.LogInformation($"{DateTime.Now} ---> Created a student user and added register request to db");

        return userCredentials.UserUniversityEmail;
    }

    public async Task<string> RegisterProfessorUserAsync(int professorRegisterRequestId)
    {
        var existingRegisterRequest = await _professorRegisterRequestService.GetRegisterRequestByIdAsync(professorRegisterRequestId);
        
        var userCredentials = CreateCredentials(existingRegisterRequest.FirstName, existingRegisterRequest.LastName, UserType.Professor);
        
        var passwordData = _passwordManager.HashPassword(userCredentials.TemporaryPassword);
        
        //Send credentials via email
        await SendUserCredentialsAsync(userCredentials.UserName, existingRegisterRequest.Email, userCredentials.TemporaryPassword);
        
        //Add a new user
        await CreateUserAsync(userCredentials, passwordData, existingRegisterRequest.Email, UserType.Professor);
        
        //Add professor in university scheduler api
        var universitySchedulerStudentService = _grpcClientService.GetService<IProfessorGrpcService>();
        await universitySchedulerStudentService.AddProfessorAsync(new ProfessorInputDataContract
        {
            FirstName = existingRegisterRequest.FirstName,
            LastName = existingRegisterRequest.LastName,
            Email = userCredentials.UserUniversityEmail,
            BirthdayDate = existingRegisterRequest.Birthdate.ToDateTime(default, DateTimeKind.Utc),
            PhoneNumber = existingRegisterRequest.PhoneNumber,
            Speciality = existingRegisterRequest.Speciality
        });
        
        //Get rid off the current register request
        await _professorRegisterRequestService.DeleteRegisterRequest(existingRegisterRequest);

        _logger.LogInformation($"{DateTime.Now} ---> Created a professor user and added register request to db");

        return userCredentials.UserUniversityEmail;    
    }

    public async Task<TokenModel> LoginUserAsync(LoginRequest request)
    {
        var existingUserWithUserName = await _userService.GetUserByQueryAsync(user => user.UserName == request.Username);
        if (existingUserWithUserName is null)
        {
            throw new UserNotRegisteredException($"There is no registered user having the username {request.Username}!");
        }
        
        if (!_passwordManager.VerifyPassword(request.Password, existingUserWithUserName.HashedPassword, existingUserWithUserName.PasswordSalt))
        {
            throw new WrongUserCredentialsException($"Wrong password for the user with username {request.Username}!");
        }

        return await _userAuthenticator.AuthenticateAsync(existingUserWithUserName);
    }

    public async Task<TokenModel> RefreshTokenAsync(RefreshRequest request)
    {
        _refreshTokenValidator.Validate(request.RefreshToken);

        var existingRefreshToken =
            await _refreshTokenService.GetRefreshTokenByQuery(refreshToken =>
                refreshToken.RefreshTokenValue == request.RefreshToken);

        if (existingRefreshToken is null)
        {
            throw new EntityNotFoundException("Couldn't find refresh token with the specified value!");
        }
        
        var existingUser = await _userService.GetUserByIdAsync(existingRefreshToken.UserId);
        await _refreshTokenService.DeleteRefreshTokenByIdAsync(existingRefreshToken.Id);

        return await _userAuthenticator.AuthenticateAsync(existingUser);
    }

    public async Task LogoutUserAsync(string? userId)
    {
        if (userId is null)
        {
            throw new UserClaimNotFoundException("Could not find the user id claim");
        }
        
        await _refreshTokenService.DeleteRefreshTokenByUserIdAsync(int.Parse(userId));
    }

    private async Task SendUserCredentialsAsync(string userName, string receiverEmail, string temporaryPassword)
    {
        var credentialsService = _grpcClientService.GetService<ICredentialsGrpcService>();
        await credentialsService.SendUserCredentialsAsync(new UserDataContract
        {
            UserName = userName,
            ReceiverEmail = receiverEmail,
            TemporaryPassword = temporaryPassword
        });
        _logger.LogInformation($"{DateTime.Now} ---> Sent credentials to specified email!");
    }

    private UserCredentials CreateCredentials(string userFirstName, string userLastName, UserType userRole)
    {
        var userCode = Guid.NewGuid().ToString("N")[..4];
        
        var userFinalEmail = userRole switch
        {
            UserType.Professor => $"{userFirstName.Replace(" ", "").ToLower()}{userLastName.Replace(" ", "").ToLower()}{userCode}@ArasakaProfessor.com",
            UserType.Student => $"{userFirstName.Replace(" ", "").ToLower()}{userLastName.Replace(" ", "").ToLower()}{userCode}@ArasakaStudent.com",
            _ => string.Empty
        };

        var userName = $"{userFirstName.Replace(" ", "").ToLower()}." +
                       $"{userLastName.Replace(" ", "")[..2].ToLower()}" +
                       $"{userCode}";

        var userTemporaryPassword = _passwordManager.GenerateTemporaryPassword(Random.Shared.Next(6, 12));

        return new UserCredentials
        {
            UserName = userName,
            UserUniversityEmail = userFinalEmail,
            TemporaryPassword = userTemporaryPassword
        };
    }

    private async Task CreateUserAsync(UserCredentials userCredentials, PasswordData passwordData, string personalEmail, UserType userRole)
    {
        var newUser = new User
        {
            UserName = userCredentials.UserName,
            Email = userCredentials.UserUniversityEmail,
            HashedPassword = passwordData.HashedPassword,
            PasswordSalt = passwordData.PasswordSalt,
            Role = userRole,
            PersonalEmail = personalEmail
        };
        
        newUser.ValidateEntity();
        
        await _userService.AddUserAsync(newUser);
    }
}