using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;

public interface ICourseService
{
    public Task<Course?> GetCourseByQueryAsync(Expression<Func<Course, bool>> query);
    public Task<Course> GetCourseByIdAsync(int id);
    public Task<List<Course>> GetAllCoursesAsync();
    public Task<DatabaseFeedback<Course>> GetOrderedCoursesAsync(PaginationSetting paginationSetting);
    public Task<List<Course>> GetCoursesByQueryAsync(Expression<Func<Course, bool>> query);
    public Task<DatabaseFeedback<Course>> GetFilteredCoursesAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<Course>> GetFilteredAndOrderedCoursesAsync(FilterOrderSettings settings);

    public Task QueueAddCourseAsync(Course course);
    public Task QueueAddCoursesAsync(List<Course> courses);
    public Task QueueUpdateCourseByIdAsync(int id, Course course, string updatedCourseJson);
    public Task QueueDeleteCourseByIdAsync(int id);
    public void QueueDeleteAllCourses();

    public Task AddCourseAsync(Course course);
    public Task AddCoursesAsync(List<Course> courses);
    public Task UpdateCourseByIdAsync(int id, Course course, string updatedCourseJson);
    public Task DeleteCourseByIdAsync(int id);
    public Task DeleteAllCoursesAsync();
}