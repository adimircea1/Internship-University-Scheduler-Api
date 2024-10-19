using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using OnEntitySharedLogic.Models;

namespace Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;

public interface IProfessorRegisterRequestService
{
    public Task<ProfessorRegisterRequest> GetRegisterRequestByIdAsync(int id);
    public Task<List<ProfessorRegisterRequest>> GetAllRegisterRequests();
    public Task<DatabaseFeedback<ProfessorRegisterRequest>> GetOrderedRegisterRequestsAsync(PaginationSetting paginationSetting);
    public Task<DatabaseFeedback<ProfessorRegisterRequest>> GetFilteredRegisterRequestsAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<ProfessorRegisterRequest>> GetFilteredAndOrderedRegisterRequestsAsync(FilterOrderSettings settings);
    public Task<ProfessorRegisterRequest?> GetRegisterRequestByEmailAsync(string email);
    public Task AddRegisterRequestAsync(ProfessorRegisterRequest requestService);
    public Task DeleteRegisterRequestByIdAsync(int id);
    public Task DeleteRegisterRequest(ProfessorRegisterRequest studentRegisterRequestService);
    public Task QueueAddRegisterRequestAsync(ProfessorRegisterRequest requestService);
    public Task QueueDeleteRegisterRequestByIdAsync(int id);
    public void QueueDeleteRegisterRequest(ProfessorRegisterRequest studentRegisterRequestService);
}