using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using OnEntitySharedLogic.Models;

namespace Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;

public interface IStudentRegisterRequestService
{
    public Task<StudentRegisterRequest> GetRegisterRequestByIdAsync(int id);
    public Task<List<StudentRegisterRequest>> GetAllRegisterRequests();
    public Task<DatabaseFeedback<StudentRegisterRequest>> GetOrderedRegisterRequestsAsync(PaginationSetting paginationSetting);
    public Task<DatabaseFeedback<StudentRegisterRequest>> GetFilteredRegisterRequestsAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<StudentRegisterRequest>> GetFilteredAndOrderedRegisterRequestsAsync(FilterOrderSettings settings);
    public Task<StudentRegisterRequest?> GetRegisterRequestByEmailAsync(string email);
    public Task AddRegisterRequestAsync(StudentRegisterRequest request);
    public Task DeleteRegisterRequestByIdAsync(int id);
    public Task DeleteRegisterRequest(StudentRegisterRequest studentRegisterRequest);
    public Task QueueAddRegisterRequestAsync(StudentRegisterRequest request);
    public Task QueueDeleteRegisterRequestByIdAsync(int id);
    public void QueueDeleteRegisterRequest(StudentRegisterRequest studentRegisterRequest);

}