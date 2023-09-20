using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses;

[Registration(Type = RegistrationKind.Scoped)]
public class StudentService : IStudentService
{
    private readonly IDatabaseGenericRepository<Student> _studentRepository;
    private readonly ILogger<StudentService> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;

    public StudentService(
        IDatabaseGenericRepository<Student> studentRepository, 
        ILogger<StudentService> logger, 
        IExpressionBuilder expressionBuilder,
        IHttpContextAccessor httpContextAccessor,
        IServiceProvider serviceProvider)
    {
        _studentRepository = studentRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
    }

    public async Task<Student?> GetStudentByQueryAsync(Expression<Func<Student, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a student by query has been made!");
        return await _studentRepository.GetEntityByQueryAsync(query);
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving student with id {id} has been made");
        return await _studentRepository.GetEntityByQueryAsync(student => student.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find student with id {id}");
    }

    public async Task<Student> GetStudentByClaimIdAsync()
    {
        var studentId = _httpContextAccessor.GetUserIdClaim();
        if (studentId is null)
        {
            throw new EntityNotFoundException($"Id {studentId} does not exist for any users!");
        }
        
        return await GetStudentByIdAsync((int)studentId);
    }

    public async Task<List<Student>> GetAllStudentsAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all students method has been called!");
        return await _studentRepository.GetAllEntitiesAsync();
    }

    public async Task<Student?> GetStudentByEmailAsync(string email)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a student has been made!");
        return await _studentRepository.GetEntityByQueryAsync(student => student.Email == email);
    }

    public async Task<List<Student>> GetStudentsByQueryAsync(Expression<Func<Student, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of students by query has been made!");
        return await _studentRepository.GetEntitiesByQueryAsync(query);
    }
    
    public async Task<DatabaseFeedback<Student>> GetOrderedStudentsAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered students by {paginationSetting.OrderBy} property has been made!");
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Student>(paginationSetting.OrderBy);
        return await _studentRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }

    public async Task<DatabaseFeedback<Student>> GetFilteredStudentsAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered students has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var studentFilter = _serviceProvider.GetRequiredService<IFilter<Student>>();
        return await _studentRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, studentFilter);
    }

    public async Task<DatabaseFeedback<Student>> GetFilteredAndOrderedStudentsAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered students has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Student>(settings.OrderBy);
        var studentFilter = _serviceProvider.GetRequiredService<IFilter<Student>>();
        return await _studentRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, studentFilter);
    }

    public async Task AddStudentAsync(Student student)
    {
        await QueueAddStudentAsync(student);
        await _studentRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a student!");
    }

    public async Task AddStudentsAsync(List<Student> students)
    {
        await QueueAddStudentsAsync(students);
        await _studentRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of students!");
    }

    public async Task UpdateStudentByIdAsync(int id, Student student, string updatedStudentJson)
    {
        await QueueUpdateStudentByIdAsync(id, student, updatedStudentJson);
        await _studentRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated student with id {id}!");
    }

    public async Task DeleteStudentByIdAsync(int id)
    {
        await QueueDeleteStudentByIdAsync(id);
        await _studentRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a student having the id {id}!");
    }

    public async Task DeleteAllStudentsAsync()
    {
        QueueDeleteAllStudents();
        await _studentRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all students!");
    }

    public async Task QueueAddStudentAsync(Student student)
    {
        student.ValidateEntity();
        await _studentRepository.AddEntityAsync(student);
    }

    public async Task QueueAddStudentsAsync(List<Student> students)
    {
        foreach (var student in students)
        {
            student.ValidateEntity();
            await _studentRepository.AddEntityAsync(student);
        }
    }

    public async Task QueueUpdateStudentByIdAsync(int id, Student student, string updatedStudentJson)
    {
        var existingStudent = await GetStudentByIdAsync(id);
        student.ValidateEntity();
        _studentRepository.UpdateEntity(existingStudent, updatedStudentJson);
    }

    public async Task QueueDeleteStudentByIdAsync(int id)
    {
        var existingStudent = await GetStudentByIdAsync(id);
        _studentRepository.DeleteEntity(existingStudent);
    }

    public void QueueDeleteAllStudents()
    {
        _studentRepository.DeleteAllEntities();
    }
}