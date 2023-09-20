using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;

public interface IProfessorService
{
    public Task<Professor?> GetProfessorByQueryAsync(Expression<Func<Professor, bool>> query);
    public Task<Professor> GetProfessorByIdAsync(int id);
    public Task<Professor> GetProfessorByIdClaimAsync();
    public Task<List<Professor>> GetAllProfessorsAsync();
    public Task<DatabaseFeedback<Professor>> GetOrderedProfessorsAsync(PaginationSetting paginationSetting);
    public Task<List<Professor>> GetProfessorsByQueryAsync(Expression<Func<Professor, bool>> query);
    public Task<DatabaseFeedback<Professor>> GetFilteredProfessorsAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<Professor>> GetFilteredAndOrderedProfessorsAsync(FilterOrderSettings settings);

    public Task QueueAddProfessorAsync(Professor professor);
    public Task QueueAddProfessorsAsync(IEnumerable<Professor> professors);
    public Task QueueUpdateProfessorByIdAsync(int id, Professor professor, string updatedProfessorJson);
    public Task QueueDeleteProfessorByIdAsync(int id);
    public void QueueDeleteAllProfessors();


    public Task AddProfessorAsync(Professor professor);
    public Task AddProfessorsAsync(List<Professor> professors);
    public Task UpdateProfessorByIdAsync(int id, Professor professor, string updatedProfessorJson);
    public Task DeleteProfessorByIdAsync(int id);
    public Task DeleteAllProfessorsAsync();
}