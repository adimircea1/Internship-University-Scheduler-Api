using Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;
using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class StudentRegisterRequestService : IStudentRegisterRequestService
{
    private readonly IDatabaseGenericRepository<StudentRegisterRequest> _registerRequestRepository;
    private readonly ILogger<StudentRegisterRequestService> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IUserService _userService;
    private readonly IServiceProvider _serviceProvider;

    public StudentRegisterRequestService(
        IDatabaseGenericRepository<StudentRegisterRequest> registerRequestRepository, 
        ILogger<StudentRegisterRequestService> logger,
        IExpressionBuilder expressionBuilder,
        IUserService userService, 
        IServiceProvider serviceProvider)
    {
        _registerRequestRepository = registerRequestRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _userService = userService;
        _serviceProvider = serviceProvider;
    }

    public async Task<StudentRegisterRequest> GetRegisterRequestByIdAsync(int id)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving a student register request by id {id} has been made!");
        return await _registerRequestRepository.GetEntityByQueryAsync(request => request.Id == id)
               ?? throw new EntityNotFoundException($"Could not find register request with id {id}");
    }
    
    public async Task<List<StudentRegisterRequest>> GetAllRegisterRequests()
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving all student register requests has been made!");
        return await _registerRequestRepository.GetAllEntitiesAsync();
    }
    
    public async Task<DatabaseFeedback<StudentRegisterRequest>> GetOrderedRegisterRequestsAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered student register requests by {paginationSetting.OrderBy} property has been made!");
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<StudentRegisterRequest>(paginationSetting.OrderBy);
        return await _registerRequestRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }

    public async Task<DatabaseFeedback<StudentRegisterRequest>> GetFilteredRegisterRequestsAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered student register requests has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var studentRegisterRequestFilter = _serviceProvider.GetRequiredService<IFilter<StudentRegisterRequest>>();
        return await _registerRequestRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, studentRegisterRequestFilter);
    }

    public async Task<DatabaseFeedback<StudentRegisterRequest>> GetFilteredAndOrderedRegisterRequestsAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered student register requests has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<StudentRegisterRequest>(settings.OrderBy);
        var studentRegisterRequestFilter = _serviceProvider.GetRequiredService<IFilter<StudentRegisterRequest>>();
        return await _registerRequestRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, studentRegisterRequestFilter);
    }

    public async Task<StudentRegisterRequest?> GetRegisterRequestByEmailAsync(string email)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving a student register request by email {email} has been made!");
        return await _registerRequestRepository.GetEntityByQueryAsync(request => request.Email == email);
    }
    
    public async Task AddRegisterRequestAsync(StudentRegisterRequest request)
    {
        await QueueAddRegisterRequestAsync(request);
        await _registerRequestRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added a new student register request!");
    }

    public async Task DeleteRegisterRequestByIdAsync(int id)
    {
        await QueueDeleteRegisterRequestByIdAsync(id);
        await _registerRequestRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully deleted student register request with id {id}!");
    }
    
    public async Task DeleteRegisterRequest(StudentRegisterRequest studentRegisterRequest)
    {
        QueueDeleteRegisterRequest(studentRegisterRequest);
        await _registerRequestRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully deleted student register request!");
    }

    public async Task QueueAddRegisterRequestAsync(StudentRegisterRequest request)
    {
        //create a validation extension for register request
        //validate if text related fields may contain (or not) other characters than letters and white spaces
        //request.ValidateRequest();
        
        //verify if a register request was already made by using the current email
        var existingRegisterRequest = await GetRegisterRequestByEmailAsync(request.Email);
        if (existingRegisterRequest is not null)
        {
            throw new RegisterRequestAlreadyMadeException($"An user having email {request.Email} already made a register request!");
        }
        
        //now verify if someone tries to make a register request with an existing user email 
        var existingUserByEmail = await _userService.GetUserByEmailAsync(request.Email);
        if (existingUserByEmail is not null)
        {
            throw new UserAlreadyRegisteredException($"An user having email {existingUserByEmail.Email} already applied!");
        }
        
        await _registerRequestRepository.AddEntityAsync(request);
    }

    public async Task QueueDeleteRegisterRequestByIdAsync(int id)
    {
        var existentRequest = await GetRegisterRequestByIdAsync(id);
        _registerRequestRepository.DeleteEntity(existentRequest);
    }
    
    public void QueueDeleteRegisterRequest(StudentRegisterRequest studentRegisterRequest)
    {
        _registerRequestRepository.DeleteEntity(studentRegisterRequest);
    }
}