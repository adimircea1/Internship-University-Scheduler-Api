using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;

public interface IAttendanceService
{
    public Task<Attendance?> GetAttendanceByQueryAsync(Expression<Func<Attendance, bool>> query);
    public Task<Attendance> GetAttendanceByIdAsync(int id);
    public Task<List<Attendance>> GetAllAttendancesAsync();
    public Task<DatabaseFeedback<Attendance>> GetOrderedAttendancesAsync(PaginationSetting paginationSetting);
    public Task<List<Attendance>> GetAttendancesByQueryAsync(Expression<Func<Attendance, bool>> query);
    public Task<DatabaseFeedback<Attendance>> GetFilteredAttendancesAsync(FilteringSettings filteringSettings);
    public Task<List<Attendance>> GetAttendancesOfStudentAsync(int studentId);
    public Task<List<Attendance>> GetCourseAttendancesAsync(int courseId);
    public Task<DatabaseFeedback<Attendance>> GetFilteredAndOrderedAttendancesAsync(FilterOrderSettings settings);

    public Task QueueAddAttendanceAsync(Attendance attendance);
    public Task QueueAddAttendancesAsync(List<Attendance> attendances);
    public Task QueueUpdateAttendanceByIdAsync(int id, Attendance attendance, string updatedAttendanceJson);
    public Task QueueDeleteAttendanceByIdAsync(int id);
    public void QueueDeleteAllAttendances();

    public Task AddAttendanceAsync(Attendance attendance);
    public Task AddAttendancesAsync(List<Attendance> attendances);
    public Task UpdateAttendanceByIdAsync(int id, Attendance attendance, string updatedAttendanceJson);
    public Task DeleteAttendanceByIdAsync(int id);
    public Task DeleteAllAttendancesAsync();
}