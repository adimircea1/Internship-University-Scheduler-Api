using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;

public interface IStudentService
{
    public Task<Student?> GetStudentByQueryAsync(Expression<Func<Student, bool>> query);
    public Task<Student> GetStudentByIdAsync(int id);
    public Task<Student> GetStudentByClaimIdAsync();
    public Task<List<Student>> GetAllStudentsAsync();
    public Task<Student?> GetStudentByEmailAsync(string email);
    public Task<List<Student>> GetStudentsByQueryAsync(Expression<Func<Student, bool>> query);
    public Task<DatabaseFeedback<Student>> GetOrderedStudentsAsync(PaginationSetting paginationSetting);
    public Task<DatabaseFeedback<Student>> GetFilteredStudentsAsync(FilteringSettings filteringSettings);
    public Task<DatabaseFeedback<Student>> GetFilteredAndOrderedStudentsAsync(FilterOrderSettings settings); 

    public Task QueueAddStudentAsync(Student student);
    public Task QueueAddStudentsAsync(List<Student> students);
    public Task QueueUpdateStudentByIdAsync(int id, Student student, string updatedStudentJson);

    public Task QueueDeleteStudentByIdAsync(int id);
    public void QueueDeleteAllStudents();

    public Task AddStudentAsync(Student student);
    public Task AddStudentsAsync(List<Student> students);
    public Task UpdateStudentByIdAsync(int id, Student student, string updatedStudentJson);

    public Task DeleteStudentByIdAsync(int id);
    public Task DeleteAllStudentsAsync();
}