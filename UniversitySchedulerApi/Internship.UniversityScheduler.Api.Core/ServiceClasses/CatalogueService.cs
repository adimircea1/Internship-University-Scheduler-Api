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
public class CatalogueService : ICatalogueService
{
    private readonly IExpressionBuilder _expressionBuilder;
    private readonly IDatabaseGenericRepository<Catalogue> _catalogueRepository;
    private readonly ILogger<CatalogueService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CatalogueService(
        IDatabaseGenericRepository<Catalogue> catalogueRepository, 
        ILogger<CatalogueService> logger, 
        IExpressionBuilder expressionBuilder, 
        IServiceProvider serviceProvider)
    {
        _catalogueRepository = catalogueRepository;
        _logger = logger;
        _expressionBuilder = expressionBuilder;
        _serviceProvider = serviceProvider;
    }

    public async Task<Catalogue?> GetCatalogueByQueryAsync(Expression<Func<Catalogue, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a catalogue by query has been made!");
        return await _catalogueRepository.GetEntityByQueryAsync(query);
    }
    
    public async Task<Catalogue> GetCatalogueByIdAsync(int id)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a catalogue with id {id} has been made!");
        return await _catalogueRepository.GetEntityByQueryAsync(catalogue => catalogue.Id == id) ??
               throw new EntityNotFoundException($"Couldn't find catalogue with id {id}");
    }
    
    public async Task<DatabaseFeedback<Catalogue>> GetOrderedCataloguesAsync(PaginationSetting paginationSetting)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of ordered catalogues by {paginationSetting.OrderBy} property has been made!");
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Catalogue>(paginationSetting.OrderBy);
        var numberOfEntitiesToSkip = (paginationSetting.PageNumber - 1) * paginationSetting.PageSize;
        return await _catalogueRepository.GetOrderedEntitiesAsync(numberOfEntitiesToSkip, paginationSetting.PageSize, orderByExpression, paginationSetting.OrderDirection);
    }


    public async Task<List<Catalogue>> GetAllCataloguesAsync()
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> Get all catalogues method has been called!");
        return await _catalogueRepository.GetAllEntitiesAsync();
    }

    public async Task<List<Catalogue>> GetCataloguesByQueryAsync(Expression<Func<Catalogue, bool>> query)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of catalogue by query has been made!");
        return await _catalogueRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task<DatabaseFeedback<Catalogue>> GetFilteredCataloguesAsync(FilteringSettings filteringSettings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered catalogues has been made!");
        var numberOfEntitiesToSkip = (filteringSettings.PageNumber - 1) * filteringSettings.PageSize;
        var catalogueFilter = _serviceProvider.GetRequiredService<IFilter<Catalogue>>();
        return await _catalogueRepository.GetFilteredEntitiesAsync(numberOfEntitiesToSkip, filteringSettings.PageSize, filteringSettings.FilterBy, catalogueFilter);
    }

    public async Task<DatabaseFeedback<Catalogue>> GetFilteredAndOrderedCataloguesAsync(FilterOrderSettings settings)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a list of filtered and ordered catalogues has been made!");
        var numberOfEntitiesToSkip = (settings.PageNumber - 1) * settings.PageSize;
        var orderByExpression = _expressionBuilder.BuildOrderByExpression<Catalogue>(settings.OrderBy);
        var catalogueFilter = _serviceProvider.GetRequiredService<IFilter<Catalogue>>();
        return await _catalogueRepository.GetFilteredAndOrderedEntitiesAsync(numberOfEntitiesToSkip, settings.PageSize, orderByExpression, settings.OrderDirection, settings.FilterBy, catalogueFilter);
    }
    
    public async Task<Catalogue> GetCatalogueByUniversityGroupIdAsync(int groupId)
    {
        _logger.LogInformation($"\n{DateTime.Now} ---> An attempt of retrieving a catalogue with university group id {groupId} has been made!");
        return await _catalogueRepository.GetEntityByQueryAsync(catalogue => catalogue.UniversityGroupId == groupId)
               ?? throw new EntityNotFoundException($"Could not find catalogue by university group id {groupId}!");
    }

    public async Task AddCatalogueAsync(Catalogue catalogue)
    {
        await QueueAddCatalogueAsync(catalogue);
        await _catalogueRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a catalogue!");
    }

    public async Task AddCataloguesAsync(List<Catalogue> catalogues)
    {
        await QueueAddCataloguesAsync(catalogues);
        await _catalogueRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully added a list of catalogues!");
    }

    public async Task UpdateCatalogueByIdAsync(int id, Catalogue catalogue, string updatedCatalogueJson)
    {
        await QueueUpdateCatalogueByIdAsync(id, catalogue, updatedCatalogueJson);
        await _catalogueRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully updated catalogue with id {id}");
    }

    public async Task DeleteCatalogueByIdAsync(int id)
    {
        await QueueDeleteCatalogueByIdAsync(id);
        await _catalogueRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a catalogue having the id {id}");
    }

    public async Task DeleteAllCataloguesAsync()
    {
        QueueDeleteAllCatalogues();
        await _catalogueRepository.SaveChangesAsync();
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted all catalogues");
    }

    public async Task QueueAddCatalogueAsync(Catalogue catalogue)
    {
        catalogue.ValidateEntity();
        await _catalogueRepository.AddEntityAsync(catalogue);
    }

    public async Task QueueAddCataloguesAsync(List<Catalogue> catalogues)
    {
        foreach (var catalogue in catalogues)
        {
            catalogue.ValidateEntity();
            await _catalogueRepository.AddEntityAsync(catalogue);
        }
    }

    public async Task QueueUpdateCatalogueByIdAsync(int id, Catalogue catalogue, string updatedCatalogueJson)
    {
        var existingCatalogue = await GetCatalogueByIdAsync(id);
        catalogue.ValidateEntity();
        _catalogueRepository.UpdateEntity(existingCatalogue, updatedCatalogueJson);
    }

    public async Task QueueDeleteCatalogueByIdAsync(int id)
    {
        var existingCatalogue = await GetCatalogueByIdAsync(id);
        _catalogueRepository.DeleteEntity(existingCatalogue);
    }

    public void QueueDeleteAllCatalogues()
    {
        _catalogueRepository.DeleteAllEntities();
    }
}