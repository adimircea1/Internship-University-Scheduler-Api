using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;

public interface IUniversityGroupService
{
    public Task<UniversityGroup?> GetUniversityGroupByQueryAsync(Expression<Func<UniversityGroup, bool>> query);
    public Task<UniversityGroup> GetUniversityGroupByIdAsync(int id);
    public Task<List<UniversityGroup>> GetAllUniversityGroupsAsync();
    public Task<DatabaseFeedback<UniversityGroup>> GetOrderedUniversityGroupsAsync(PaginationSetting paginationSetting);
    public Task<List<UniversityGroup>> GetUniversityGroupsByQueryAsync(Expression<Func<UniversityGroup, bool>> query);
    public Task<DatabaseFeedback<UniversityGroup>> GetFilteredUniversityGroupsAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<Student>> GetOrderedAndFilteredStudentsFromGroupAsync(FilterOrderSettings settings, int id);
    public Task<DatabaseFeedback<UniversityGroup>> GetFilteredAndOrderedUniversityGroupsAsync(FilterOrderSettings settings);


    public Task QueueAddUniversityGroupAsync(UniversityGroup universityGroup);
    public Task QueueAddUniversityGroupsAsync(List<UniversityGroup> universityGroups);
    public Task QueueUpdateUniversityGroupByIdAsync(int id, UniversityGroup universityGroup, string updatedUniversityGroupJson);
    public Task QueueDeleteUniversityGroupByIdAsync(int id);
    public void QueueDeleteAllUniversityGroups();
    public Task QueueAddStudentInGroupAsync(int studentId, int groupId);
    public Task QueueRemoveStudentFromGroupAsync(int studentId, int groupId);
    public Task QueueRemoveAllStudentsFromGroupAsync(int groupId);
    public Task<bool> QueueAddMultipleStudentsInGroupAsync(int groupId, List<int> studentIds);

    public Task AddUniversityGroupAsync(UniversityGroup universityGroup);
    public Task AddUniversityGroupsAsync(List<UniversityGroup> universityGroups);
    public Task UpdateUniversityGroupByIdAsync(int id, UniversityGroup universityGroup, string updatedUniversityGroupJson);
    public Task DeleteUniversityGroupByIdAsync(int id);
    public Task DeleteAllUniversityGroupsAsync();
    public Task AddStudentInGroupAsync(int studentId, int groupId);
    public Task RemoveStudentFromGroupAsync(int studentId, int groupId);
    public Task AddMultipleStudentsInGroupAsync(int groupId, List<int> studentIds);
    public Task RemoveAllStudentsFromGroupAsync(int groupId);
}