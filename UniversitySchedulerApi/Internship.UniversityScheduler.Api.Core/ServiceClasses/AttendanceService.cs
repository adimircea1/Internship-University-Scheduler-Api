using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
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
public class AttendanceService : IAttendanceService
{
    private readonly IDatabaseGenericRepository<Attendance> _attendanceRepository;
    private readonly ICourseService _courseService;
    private readonly IStudentService _studentService;
    private readonly ILogger<AttendanceService> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IServiceProvider _serviceProvider;

    public AttendanceService(
        IDatabaseGenericRepository<Attendance> attendanceRepository,
        IStudentService studentService,
        ICourseService courseService,
        ILogger<AttendanceService> logger,
        IExpressionBuilder expressionBuilder,
        IServiceProvider serviceProvider)
    {
        _attendanceRepository = attendanceRepository;
        _studentService = studentService;
        _courseService = courseService;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _serviceProvider = serviceProvider;
    }

    public async Task<Attendance?> GetAttendanceByQueryAsync(Expression<Func<Attendance, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving attendance by query has been made");
        return await _attendanceRepository.GetEntityByQueryAsync(query);
    }

    public async Task<Attendance> GetAttendanceByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving attendance with id {id} has been made");
        return await _attendanceRepository.GetEntityByQueryAsync(attendance => attendance.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find attendance with id {id}");
    }

    public async Task<List<Attendance>> GetAllAttendancesAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all attendances method has been called!");
        return await _attendanceRepository.GetAllEntitiesAsync();
    }

    public async Task<DatabaseFeedback<Attendance>> GetOrderedAttendancesAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered attendances by {paginationSetting.OrderBy} property has been made!");
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Attendance>(paginationSetting.OrderBy);
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        return await _attendanceRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }

    public async Task<List<Attendance>> GetAttendancesByQueryAsync(Expression<Func<Attendance, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of attendances by query has been made");
        return await _attendanceRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<DatabaseFeedback<Attendance>> GetFilteredAttendancesAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered attendances has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var attendanceFilter = _serviceProvider.GetRequiredService<IFilter<Attendance>>();
        return await _attendanceRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, attendanceFilter);
    }

    public async Task<DatabaseFeedback<Attendance>> GetFilteredAndOrderedAttendancesAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered attendances has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Attendance>(settings.OrderBy);
        var attendanceFilter = _serviceProvider.GetRequiredService<IFilter<Attendance>>();
        return await _attendanceRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, attendanceFilter);
    }

    public async Task<List<Attendance>> GetAttendancesOfStudentAsync(int studentId)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all attendances of student method has been called!");
        var student = await _studentService.GetStudentByIdAsync(studentId);
        return await GetAttendancesByQueryAsync(attendance => attendance.StudentId == studentId);
    }

    public async Task<List<Attendance>> GetCourseAttendancesAsync(int courseId)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all attendances of course method has been called!");
        var course = await _courseService.GetCourseByIdAsync(courseId);
        return await GetAttendancesByQueryAsync(attendance => attendance.CourseId == courseId);
    }


    public async Task AddAttendanceAsync(Attendance attendance)
    {
        await QueueAddAttendanceAsync(attendance);
        await _attendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added an attendance!");
    }

    public async Task AddAttendancesAsync(List<Attendance> attendances)
    {
        await QueueAddAttendancesAsync(attendances);
        await _attendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of attendances!");
    }

    public async Task UpdateAttendanceByIdAsync(int id, Attendance updatedAttendance, string updatedAttendanceJson)
    {
        await QueueUpdateAttendanceByIdAsync(id, updatedAttendance, updatedAttendanceJson);
        await _attendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated attendance with id {id}!");
    }

    public async Task DeleteAttendanceByIdAsync(int id)
    {
        await QueueDeleteAttendanceByIdAsync(id);
        await _attendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted attendance with id {id}!");
    }

    public async Task DeleteAllAttendancesAsync()
    {
        QueueDeleteAllAttendances();
        await _attendanceRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all attendance records!");
    }

    public async Task QueueAddAttendanceAsync(Attendance attendance)
    {
        attendance.ValidateEntity();
        await _attendanceRepository.AddEntityAsync(attendance);
    }

    public async Task QueueAddAttendancesAsync(List<Attendance> attendances)
    {
        foreach (var attendance in attendances)
        {
            attendance.ValidateEntity();
            await _attendanceRepository.AddEntityAsync(attendance);
        }
    }

    public async Task QueueUpdateAttendanceByIdAsync(int id, Attendance attendance, string updatedAttendanceJson)
    {
        var existingAttendance = await GetAttendanceByIdAsync(id);
        attendance.ValidateEntity();
        _attendanceRepository.UpdateEntity(existingAttendance, updatedAttendanceJson);
    }

    public async Task QueueDeleteAttendanceByIdAsync(int id)
    {
        var existingAttendance = await GetAttendanceByIdAsync(id);
        _attendanceRepository.DeleteEntity(existingAttendance);
    }

    public void QueueDeleteAllAttendances()
    {
        _attendanceRepository.DeleteAllEntities();
    }
}