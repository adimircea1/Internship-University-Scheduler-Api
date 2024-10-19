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
public class ProfessorService : IProfessorService
{
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IDatabaseGenericRepository<Professor> _professorRepository;
    private readonly ILogger<ProfessorService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;

    public ProfessorService(
        IDatabaseGenericRepository<Professor> professorRepository, 
        ILogger<ProfessorService> logger, 
        IExpressionBuilder expressionBuilder,
        IHttpContextAccessor httpContextAccessor, 
        IServiceProvider serviceProvider)
    {
        _professorRepository = professorRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
    }

    public async Task<Professor?> GetProfessorByQueryAsync(Expression<Func<Professor, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a professor by query has been made!");
        return await _professorRepository.GetEntityByQueryAsync(query);
    }

    public async Task<Professor> GetProfessorByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a professor with id {id} has been made");
        return await _professorRepository.GetEntityByQueryAsync(professor => professor.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find professor with id {id}");
    }

    public async Task<Professor> GetProfessorByIdClaimAsync()
    {
        var professorId = _httpContextAccessor.GetUserIdClaim();
        if (professorId is null)
        {
            throw new EntityNotFoundException($"Id {professorId} does not exist for any users!");
        }
        
        return await GetProfessorByIdAsync((int)professorId);
    }
    
    public async Task<List<Professor>> GetAllProfessorsAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all professors method has been called!");
        return await _professorRepository.GetAllEntitiesAsync();
    }

    public async Task<DatabaseFeedback<Professor>> GetOrderedProfessorsAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered professors by {paginationSetting.OrderBy} property has been made!");
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Professor>(paginationSetting.OrderBy);
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        return await _professorRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }
    
    public async Task<List<Professor>> GetProfessorsByQueryAsync(Expression<Func<Professor, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of professors by query has been made!");
        return await _professorRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<DatabaseFeedback<Professor>> GetFilteredProfessorsAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered professors has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var professorFilter = _serviceProvider.GetRequiredService<IFilter<Professor>>();
        return await _professorRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, professorFilter);
    }

    public async Task<DatabaseFeedback<Professor>> GetFilteredAndOrderedProfessorsAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered professors has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Professor>(settings.OrderBy);
        var professorFilter = _serviceProvider.GetRequiredService<IFilter<Professor>>();
        return await _professorRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, professorFilter);
    }
    
    public async Task AddProfessorAsync(Professor professor)
    {
        await QueueAddProfessorAsync(professor);
        await _professorRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a professor!");
    }

    public async Task AddProfessorsAsync(List<Professor> professors)
    {
        await QueueAddProfessorsAsync(professors);
        await _professorRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of professors!");
    }

    public async Task UpdateProfessorByIdAsync(int id, Professor professor, string updatedProfessorJson)
    {
        await QueueUpdateProfessorByIdAsync(id, professor, updatedProfessorJson);
        await _professorRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated professor with id {id}!");
    }

    public async Task DeleteProfessorByIdAsync(int id)
    {
        await QueueDeleteProfessorByIdAsync(id);
        await _professorRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a professor having the id {id}!");
    }

    public async Task DeleteAllProfessorsAsync()
    {
        QueueDeleteAllProfessors();
        await _professorRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all professors!");
    }

    public async Task QueueAddProfessorAsync(Professor professor)
    {
        professor.ValidateEntity();
        await _professorRepository.AddEntityAsync(professor);
    }

    public async Task QueueAddProfessorsAsync(IEnumerable<Professor> professors)
    {
        await _professorRepository.AddEntitiesAsync(professors);
    }

    public async Task QueueUpdateProfessorByIdAsync(int id, Professor professor, string updatedProfessorJson)
    {
        var existingProfessor = await GetProfessorByIdAsync(id);
        professor.ValidateEntity();
        _professorRepository.UpdateEntity(existingProfessor, updatedProfessorJson);
    }

    public async Task QueueDeleteProfessorByIdAsync(int id)
    {
        var existingProfessor = await GetProfessorByIdAsync(id);
        _professorRepository.DeleteEntity(existingProfessor);
    }

    public void QueueDeleteAllProfessors()
    {
        _professorRepository.DeleteAllEntities();
    }
}