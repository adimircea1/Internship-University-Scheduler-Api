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
public class GradeService : IGradeService
{
    private readonly ICatalogueService _catalogueService;
    private readonly ICourseService _courseService;
    private readonly IDatabaseGenericRepository<Grade> _gradeRepository;
    private readonly IStudentService _studentService;
    private readonly ILogger<Grade> _logger;
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IServiceProvider _serviceProvider;

    public GradeService(
        IDatabaseGenericRepository<Grade> gradeRepository,
        ICatalogueService catalogueService,
        IStudentService studentService,
        ICourseService courseService, 
        ILogger<Grade> logger, 
        IExpressionBuilder expressionBuilder,
        IServiceProvider serviceProvider)
    {
        _gradeRepository = gradeRepository;
        _catalogueService = catalogueService;
        _studentService = studentService;
        _courseService = courseService;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _serviceProvider = serviceProvider;
    }

    public async Task<Grade?> GetGradeByQueryAsync(Expression<Func<Grade, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a grade by query has been made");
        return await _gradeRepository.GetEntityByQueryAsync(query);
    }

    public async Task<Grade> GetGradeByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a grade with id {id} has been made");
        return await _gradeRepository.GetEntityByQueryAsync(grade => grade.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find grade with id {id}");
    }

    public async Task<List<Grade>> GetAllGradesAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all grades method has been called!");
        return await _gradeRepository.GetAllEntitiesAsync();
    }

    public async Task<DatabaseFeedback<Grade>> GetOrderedGradesAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered grades by {paginationSetting.OrderBy} property has been made!");
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Grade>(paginationSetting.OrderBy);
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        return await _gradeRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }
    
    public async Task<List<Grade>> GetGradesByQueryAsync(Expression<Func<Grade, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of grades by query has been made");
        return await _gradeRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<DatabaseFeedback<Grade>> GetFilteredGradesAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered grades has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var gradeFilter = _serviceProvider.GetRequiredService<IFilter<Grade>>();
        return await _gradeRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, gradeFilter);
    }

    public async Task<DatabaseFeedback<Grade>> GetFilteredAndOrderedGradesAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered grades has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Grade>(settings.OrderBy);
        var gradeFilter = _serviceProvider.GetRequiredService<IFilter<Grade>>();
        return await _gradeRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, gradeFilter);
    }

    
    public async Task<List<Grade>> GetAllGradesFromCatalogueAsync(int catalogueId)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all grades from catalogue method has been called!");
        var catalogue = await _catalogueService.GetCatalogueByIdAsync(catalogueId);
        return await GetGradesByQueryAsync(grade => grade.CatalogueId == catalogueId);
    }

    public async Task<List<Grade>> GetAllGradesOfAStudentAsync(int studentId)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all grades of a student method has been called!");
        var student = await _studentService.GetStudentByIdAsync(studentId);
        return await GetGradesByQueryAsync(grade => grade.StudentId == studentId);
    }

    public async Task<List<Grade>> GetAllGradesFromCourseAsync(int courseId)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all grades from a course method has been called!");
        var course = await _courseService.GetCourseByIdAsync(courseId);
        return await GetGradesByQueryAsync(grade => grade.CourseId == courseId);
    }

    public async Task AddGradeAsync(Grade grade)
    {
        await QueueAddGradeAsync(grade);
        await _gradeRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a grade!");
    }

    public async Task AddGradesAsync(List<Grade> grades)
    {
        await QueueAddGradesAsync(grades);
        await _gradeRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of grades!");
    }

    public async Task UpdateGradeByIdAsync(int id, Grade grade, string updatedGradeJson)
    {
        await QueueUpdateGradeByIdAsync(id, grade, updatedGradeJson);
        await _gradeRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated grade with id {id}!");
    }

    public async Task DeleteGradeByIdAsync(int id)
    {
        await QueueDeleteGradeByIdAsync(id);
        await _gradeRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a grade having the id {id}!");
    }

    public async Task DeleteAllGradesAsync()
    {
        QueueDeleteAllGrades();
        await _gradeRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all grades!");
    }

    public async Task QueueAddGradeAsync(Grade grade)
    {
        grade.ValidateEntity();
        await _gradeRepository.AddEntityAsync(grade);
    }

    public async Task QueueAddGradesAsync(List<Grade> grades)
    {
        foreach (var grade in grades)
        {
            grade.ValidateEntity();
            await _gradeRepository.AddEntityAsync(grade);
        }
    }

    public async Task QueueUpdateGradeByIdAsync(int id, Grade grade, string updatedGradeJson)
    {
        var existingGrade = await GetGradeByIdAsync(id);
        grade.ValidateEntity();
        _gradeRepository.UpdateEntity(existingGrade, updatedGradeJson);
    }

    public async Task QueueDeleteGradeByIdAsync(int id)
    {
        var existingGrade = await GetGradeByIdAsync(id);
        _gradeRepository.DeleteEntity(existingGrade);
    }

    public void QueueDeleteAllGrades()
    {
        _gradeRepository.DeleteAllEntities();
    }
}