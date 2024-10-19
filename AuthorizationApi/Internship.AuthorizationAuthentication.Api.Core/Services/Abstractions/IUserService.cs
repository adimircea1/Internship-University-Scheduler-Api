using System.Linq.Expressions;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using OnEntitySharedLogic.Models;

namespace Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;

public interface IUserService
{
    public Task<User?> GetUserByQueryAsync(Expression<Func<User, bool>> query);
    public Task<User> GetUserByIdAsync(int userId);
    public Task<List<User>> GetAllUsersAsync();
    public Task<List<User>> GetUsersByQueryAsync(Expression<Func<User, bool>> query);
    public Task<DatabaseFeedback<User>> GetOrderedUsersAsync(PaginationSetting paginationSetting);
    public Task<DatabaseFeedback<User>> GetFilteredUsersAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<User>> GetFilteredAndOrderedUsersAsync(FilterOrderSettings settings);
    public Task<User> GetUserByIdClaimAsync();
    public string? GetUserRoleClaim();
    public Task<User?> GetUserByEmailAsync(string email);

    public Task QueueAddUserAsync(User user);
    public Task QueueAddUsersAsync(List<User> users);
    public Task QueueUpdateUserByIdAsync(int id, User user, string updatedUserJson);
    public Task QueueDeleteUserByIdAsync(int id);
    public void QueueDeleteAllUsers();
    public Task QueueChangeUserPasswordAsync(int userId, string currentPassword, string newPassword);

    public Task AddUserAsync(User user);
    public Task AddUsersAsync(List<User> users);
    public Task UpdateUserByIdAsync(int id, User user, string updatedUserJson);
    public Task DeleteUserByIdAsync(int id);
    public Task DeleteAllUsersAsync();
    public Task ChangeUserPasswordAsync(int userId, string currentPassword, string newPassword);
}