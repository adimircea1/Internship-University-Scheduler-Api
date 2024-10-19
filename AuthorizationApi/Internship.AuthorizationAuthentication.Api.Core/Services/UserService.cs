using System.Linq.Expressions;
using Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class UserService : IUserService
{
    private readonly IDatabaseGenericRepository<User> _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordManager _passwordManager;
    private readonly IServiceProvider _serviceProvider;
    
    public UserService(
        IDatabaseGenericRepository<User> userRepository, 
        ILogger<UserService> logger, 
        IExpressionBuilder expressionBuilder, 
        IHttpContextAccessor httpContextAccessor, 
        IPasswordManager passwordManager,
        IServiceProvider serviceProvider)
    {
        _userRepository = userRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _httpContextAccessor = httpContextAccessor;
        _passwordManager = passwordManager;
        _serviceProvider = serviceProvider;
    }

    public async Task<User?> GetUserByQueryAsync(Expression<Func<User, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving an user by query has been made!");
        return await _userRepository.GetEntityByQueryAsync(query);
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving an user with id {id} has been made!");
        
        return await _userRepository.GetEntityByQueryAsync(user => user.Id == id)
               ?? throw new EntityNotFoundException($"Couldn't find user with id {id}");
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all users method has been called!");
        return await _userRepository.GetAllEntitiesAsync();
    }

    public async Task<List<User>> GetUsersByQueryAsync(Expression<Func<User, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of users by query has been made!");
        return await _userRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<DatabaseFeedback<User>> GetOrderedUsersAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered users by {paginationSetting.OrderBy} property has been made!");
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<User>(paginationSetting.OrderBy);
        return await _userRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }

    public async Task<DatabaseFeedback<User>> GetFilteredUsersAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered users has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var userFilter = _serviceProvider.GetRequiredService<IFilter<User>>();
        return await _userRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, userFilter);
    }

    public async Task<DatabaseFeedback<User>> GetFilteredAndOrderedUsersAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered users has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<User>(settings.OrderBy);
        var userFilter = _serviceProvider.GetRequiredService<IFilter<User>>();
        return await _userRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, userFilter);
    }

    public async Task<User> GetUserByIdClaimAsync()
    {
        var userId = _httpContextAccessor.GetUserIdClaim();
        if (userId is null)
        {
            throw new EntityNotFoundException($"Id {userId} does not exist for any users!");
        }
        
        return await GetUserByIdAsync((int)userId);
    }

    public string? GetUserRoleClaim()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving user role claim has been made!");
        return _httpContextAccessor.GetUserClaimRole();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving an user by email {email} has been made!");
        return await _userRepository.GetEntityByQueryAsync(user => user.Email == email);
    }
    
    public async Task AddUserAsync(User user)
    {
        await QueueAddUserAsync(user);
        _logger.LogInformation(
            $"\n{DateTime.Now} ---> Successfully added an user!");
        await _userRepository.SaveChangesAsync();
    }

    public async Task AddUsersAsync(List<User> users)
    {
        await QueueAddUsersAsync(users);
        _logger.LogInformation(
            $"\n{DateTime.Now} ---> Successfully added a list of users!");
        await _userRepository.SaveChangesAsync();
    }

    public async Task UpdateUserByIdAsync(int id, User user, string updatedUserJson)
    {
        await QueueUpdateUserByIdAsync(id, user, updatedUserJson);
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated user with id {id}!");
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteUserByIdAsync(int id)
    {
        await QueueDeleteUserByIdAsync(id);
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted an user with id {id}!");
        await _userRepository.SaveChangesAsync();
    }

    public async Task DeleteAllUsersAsync()
    {
        QueueDeleteAllUsers();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all users!");
        await _userRepository.SaveChangesAsync();
    }

    public async Task ChangeUserPasswordAsync(int userId, string currentPassword, string newPassword)
    {
        await QueueChangeUserPasswordAsync(userId, currentPassword, newPassword);
        await _userRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully changed password for the user with id {userId}!");
    }

    public async Task QueueAddUserAsync(User user)
    {
        await _userRepository.AddEntityAsync(user);
    }

    public async Task QueueAddUsersAsync(List<User> users)
    {
        foreach (var user in users)
        {
            await _userRepository.AddEntityAsync(user);
        }
    }

    public async Task QueueChangeUserPasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var existingUser = await GetUserByIdAsync(userId);

        if (!_passwordManager.VerifyPassword(currentPassword, existingUser.HashedPassword, existingUser.PasswordSalt))
        {
            throw new WrongUserCredentialsException($"Wrong password for the user with id {existingUser.Id}!");
        }

        if (_passwordManager.VerifyPassword(newPassword, existingUser.HashedPassword, existingUser.PasswordSalt))
        {
            throw new InvalidNewPasswordException("The new password should not be the same as the old password!");
        }

        existingUser.HashedPassword = _passwordManager.HashPassword(newPassword).HashedPassword;
    }
    
    public async Task QueueUpdateUserByIdAsync(int id, User user, string updatedUserJson)
    {
        user.ValidateEntity();
        var outdatedUser = await GetUserByIdAsync(id);
        _userRepository.UpdateEntity(outdatedUser, updatedUserJson);
    }

    public async Task QueueDeleteUserByIdAsync(int id)
    {
        var existingUser = await GetUserByIdAsync(id);
        _userRepository.DeleteEntity(existingUser);
    }

    public void QueueDeleteAllUsers()
    {
        _userRepository.DeleteAllEntities();
    }
}