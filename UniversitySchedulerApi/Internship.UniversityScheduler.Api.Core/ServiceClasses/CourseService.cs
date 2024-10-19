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
public class CourseService : ICourseService
{
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IDatabaseGenericRepository<Course> _courseRepository;
    private readonly ILogger<CourseService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CourseService(
        IDatabaseGenericRepository<Course> courseRepository,
        ILogger<CourseService> logger, 
        IExpressionBuilder expressionBuilder,
        IServiceProvider serviceProvider)
    {
        _courseRepository = courseRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _serviceProvider = serviceProvider;
    }

    public async Task<Course?> GetCourseByQueryAsync(Expression<Func<Course, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a course by query has been made!");
        return await _courseRepository.GetEntityByQueryAsync(query);
    }

    public async Task<Course> GetCourseByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a course with id {id} has been made!");
        return await _courseRepository.GetEntityByQueryAsync(course => course.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find course with id {id}");
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all courses method has been called!");
        return await _courseRepository.GetAllEntitiesAsync();
    }

    public async Task<List<Course>> GetCoursesByQueryAsync(Expression<Func<Course, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of courses by query has been made!");
        return await _courseRepository.GetEntitiesByQueryAsync(query);
    }
    
    public async Task<DatabaseFeedback<Course>> GetOrderedCoursesAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered courses by {paginationSetting.OrderBy} property has been made!");
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Course>(paginationSetting.OrderBy);
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        return await _courseRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }

    public async Task<DatabaseFeedback<Course>> GetFilteredCoursesAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered courses has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var courseFilter = _serviceProvider.GetRequiredService<IFilter<Course>>();
        return await _courseRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, courseFilter);
    }

    public async Task<DatabaseFeedback<Course>> GetFilteredAndOrderedCoursesAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered courses has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Course>(settings.OrderBy);
        var courseFilter = _serviceProvider.GetRequiredService<IFilter<Course>>();
        return await _courseRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, courseFilter);
    }
    
    public async Task AddCourseAsync(Course course)
    {
        await QueueAddCourseAsync(course);
        await _courseRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a course!");
    }

    public async Task AddCoursesAsync(List<Course> courses)
    {
        await QueueAddCoursesAsync(courses);
        await _courseRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of courses!");
    }

    public async Task UpdateCourseByIdAsync(int id, Course course, string updatedCourseJson)
    {
        await QueueUpdateCourseByIdAsync(id, course, updatedCourseJson);
        await _courseRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated course with id {id}!");
    }

    public async Task DeleteCourseByIdAsync(int id)
    {
        await QueueDeleteCourseByIdAsync(id);
        await _courseRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a course having the id {id}!");
    }

    public async Task DeleteAllCoursesAsync()
    {
        QueueDeleteAllCourses();
        await _courseRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all courses!");
    }

    public async Task QueueAddCourseAsync(Course course)
    {
        course.ValidateEntity();
        await _courseRepository.AddEntityAsync(course);
    }

    public async Task QueueAddCoursesAsync(List<Course> courses)
    {
        foreach (var course in courses)
        {
            course.ValidateEntity();
            await _courseRepository.AddEntityAsync(course);
        }
    }

    public async Task QueueUpdateCourseByIdAsync(int id, Course course, string updatedCourseJson)
    {
        var existingCourse = await GetCourseByIdAsync(id);
        course.ValidateEntity();
        _courseRepository.UpdateEntity(existingCourse, updatedCourseJson);
    }

    public async Task QueueDeleteCourseByIdAsync(int id)
    {
        var existingCourse = await GetCourseByIdAsync(id);
        _courseRepository.DeleteEntity(existingCourse);
    }

    public void QueueDeleteAllCourses()
    {
        _courseRepository.DeleteAllEntities();
    }
}