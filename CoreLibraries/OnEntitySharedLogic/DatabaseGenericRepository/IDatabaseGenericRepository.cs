using System.Linq.Expressions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace OnEntitySharedLogic.DatabaseGenericRepository;

public interface IDatabaseGenericRepository<TEntity> where TEntity : IEntity
{
    public Task AddEntityAsync(TEntity entity);
    public Task AddEntitiesAsync(IEnumerable<TEntity> entities);
    public Task<TEntity?> GetEntityByQueryAsync(Expression<Func<TEntity, bool>> query);
    public Task<List<TEntity>> GetAllEntitiesAsync();
    public Task<List<TEntity>> GetEntitiesByQueryAsync(Expression<Func<TEntity, bool>> query);
    public Task<DatabaseFeedback<TEntity>> GetOrderedEntitiesAsync(int numberToSkip, int numberToTake, Expression<Func<TEntity, object>>? queryExpression, OrderByDirection direction);
    public Task<DatabaseFeedback<TEntity>> GetFilteredEntitiesAsync(int numberToSkip, int numberToTake, Dictionary<string, string> filterBy, IFilter<TEntity> entityFilter);
    public Task<DatabaseFeedback<TEntity>> GetFilteredAndOrderedEntitiesAsync(int numberToSkip, int numberToTake, Expression<Func<TEntity, object>>? queryExpression, OrderByDirection direction, 
        Dictionary<string, string> filterBy,  IFilter<TEntity> entityFilter);
    public Task<DatabaseFeedback<TEntity>> SearchEntitiesAsync(FilterObject<TEntity> filterObject, PaginationObject<TEntity>? paginationSettings);
    public Task<int> GetNumberOfEntitiesFromDatabase();
    public void DeleteEntity(TEntity entity);
    public void DeleteAllEntities();
    public void UpdateEntity(TEntity outdatedEntity, string jsonEntity);
    public Task SaveChangesAsync();
}