using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;

public interface IGradeService
{
    public Task<Grade?> GetGradeByQueryAsync(Expression<Func<Grade, bool>> query);

    public Task<Grade> GetGradeByIdAsync(int id);
    public Task<List<Grade>> GetAllGradesAsync();
    public Task<DatabaseFeedback<Grade>> GetOrderedGradesAsync(PaginationSetting paginationSetting);
    public Task<List<Grade>> GetGradesByQueryAsync(Expression<Func<Grade, bool>> query);
    public Task<DatabaseFeedback<Grade>> GetFilteredGradesAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<Grade>> GetFilteredAndOrderedGradesAsync(FilterOrderSettings settings);
    public Task<List<Grade>> GetAllGradesFromCatalogueAsync(int id);
    public Task<List<Grade>> GetAllGradesOfAStudentAsync(int studentId);
    public Task<List<Grade>> GetAllGradesFromCourseAsync(int courseId);
        
    public Task QueueAddGradeAsync(Grade grade);
    public Task QueueAddGradesAsync(List<Grade> grades);
    public Task QueueUpdateGradeByIdAsync(int id, Grade grade, string updateGradeJson);
    public Task QueueDeleteGradeByIdAsync(int id);
    public void QueueDeleteAllGrades();

    public Task AddGradeAsync(Grade grade);
    public Task AddGradesAsync(List<Grade> grades);
    public Task UpdateGradeByIdAsync(int id, Grade grade, string updateGradeJson);
    public Task DeleteGradeByIdAsync(int id);
    public Task DeleteAllGradesAsync();
}