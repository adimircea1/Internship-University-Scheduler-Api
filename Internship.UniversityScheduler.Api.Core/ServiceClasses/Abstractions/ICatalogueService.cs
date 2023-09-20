using System.Linq.Expressions;
using Internship.UniversityScheduler.Api.Core.Models;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;

public interface ICatalogueService
{
    public Task<Catalogue> GetCatalogueByIdAsync(int id);
    public Task<Catalogue?> GetCatalogueByQueryAsync(Expression<Func<Catalogue, bool>> query);

    public Task<List<Catalogue>> GetAllCataloguesAsync();
    public Task<DatabaseFeedback<Catalogue>> GetOrderedCataloguesAsync(PaginationSetting paginationSetting);
    public Task<List<Catalogue>> GetCataloguesByQueryAsync(Expression<Func<Catalogue, bool>> query);
    public Task<DatabaseFeedback<Catalogue>> GetFilteredCataloguesAsync(FilteringSettings filteringSettings);
    public Task<Catalogue> GetCatalogueByUniversityGroupIdAsync(int groupId);
    public Task<DatabaseFeedback<Catalogue>> GetFilteredAndOrderedCataloguesAsync(FilterOrderSettings settings);

    public Task QueueAddCatalogueAsync(Catalogue catalogue);
    public Task QueueAddCataloguesAsync(List<Catalogue> catalogues);
    public Task QueueUpdateCatalogueByIdAsync(int id, Catalogue catalogue, string updatedCatalogueJson);
    public Task QueueDeleteCatalogueByIdAsync(int id);
    public void QueueDeleteAllCatalogues();

    public Task AddCatalogueAsync(Catalogue catalogue);
    public Task AddCataloguesAsync(List<Catalogue> catalogues);
    public Task UpdateCatalogueByIdAsync(int id, Catalogue catalogue, string updatedCatalogueJson);
    public Task DeleteCatalogueByIdAsync(int id);
    public Task DeleteAllCataloguesAsync();
}