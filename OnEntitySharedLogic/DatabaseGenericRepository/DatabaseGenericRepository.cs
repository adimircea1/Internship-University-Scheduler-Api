using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace OnEntitySharedLogic.DatabaseGenericRepository;

public class DatabaseGenericRepository<TEntity> : IDatabaseGenericRepository<TEntity> where TEntity : class
{
    private readonly IDataContext _context;
    private readonly DbSet<TEntity> _entitySet;

    public DatabaseGenericRepository(
        IDataContext context)
    {
        _context = context;
        _entitySet = _context.Set<TEntity>();
    }

    public async Task AddEntityAsync(TEntity entity)
    {
        await _entitySet.AddAsync(entity);
    }

    public async Task AddEntitiesAsync(IEnumerable<TEntity> entities)
    {
        await _entitySet.AddRangeAsync(entities);
    }

    public async Task<TEntity?> GetEntityByQueryAsync(Expression<Func<TEntity, bool>> query)
    {
        return await _entitySet.FirstOrDefaultAsync(query);
    }

    public async Task<List<TEntity>> GetAllEntitiesAsync()
    {
        return await _entitySet.ToListAsync();
    }

    public async Task<DatabaseFeedback<TEntity>> GetOrderedEntitiesAsync(int numberToSkip, int numberToTake, Expression<Func<TEntity, object>>? queryExpression, OrderByDirection direction)
    {
        if (queryExpression is null)
        {
            return new DatabaseFeedback<TEntity>
            {
                Entities = await _entitySet.Skip(numberToSkip).Take(numberToTake).ToListAsync(),
                NumberOfEntities = await _entitySet.CountAsync()
            };
        }

        return direction switch
        {
            OrderByDirection.Ascending => new DatabaseFeedback<TEntity>
            {
                Entities = await _entitySet.OrderBy(queryExpression)
                    .Skip(numberToSkip)
                    .Take(numberToTake)
                    .ToListAsync(),
                NumberOfEntities = await _entitySet.CountAsync()
            },

            OrderByDirection.Descending => new DatabaseFeedback<TEntity>
            {
                Entities = await _entitySet.OrderByDescending(queryExpression)
                    .Skip(numberToSkip)
                    .Take(numberToTake)
                    .ToListAsync(),
                NumberOfEntities = await _entitySet.CountAsync()
            },

            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            
        };
    }

    public async Task<DatabaseFeedback<TEntity>> GetFilteredEntitiesAsync(int numberToSkip, int numberToTake, Dictionary<string, string> filterBy, IFilter<TEntity> entityFilter)
    {
        var filteredEntities = entityFilter.Filter(_entitySet, filterBy);
        return new DatabaseFeedback<TEntity>
        {
            Entities = await filteredEntities.Skip(numberToSkip).Take(numberToTake).ToListAsync(),
            NumberOfEntities = await filteredEntities.CountAsync()
        };
    }

    public async Task<DatabaseFeedback<TEntity>> GetFilteredAndOrderedEntitiesAsync(int numberToSkip, int numberToTake, Expression<Func<TEntity, object>>? queryExpression, OrderByDirection direction,
        Dictionary<string, string> filterBy, IFilter<TEntity> entityFilter)
    {
        var filteredEntities =  entityFilter.Filter(_entitySet, filterBy);
        
        if (queryExpression is null)
        {
            return new DatabaseFeedback<TEntity>
            {
                Entities = await filteredEntities.Skip(numberToSkip).Take(numberToTake).ToListAsync(),
                NumberOfEntities = await filteredEntities.CountAsync()
            };
        }

        return direction switch
        {
            OrderByDirection.Ascending => new DatabaseFeedback<TEntity>
            {
                Entities = await filteredEntities.OrderBy(queryExpression)
                    .Skip(numberToSkip)
                    .Take(numberToTake)
                    .ToListAsync(),
                NumberOfEntities = await filteredEntities.CountAsync()
            },

            OrderByDirection.Descending => new DatabaseFeedback<TEntity>
            {
                Entities = await filteredEntities.OrderByDescending(queryExpression)
                    .Skip(numberToSkip)
                    .Take(numberToTake)
                    .ToListAsync(),
                NumberOfEntities = await filteredEntities.CountAsync()
            },

            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }


    public async Task<int> GetNumberOfEntitiesFromDatabase()
    {
        return await _entitySet.CountAsync();
    }

    public async Task<List<TEntity>> GetEntitiesByQueryAsync(Expression<Func<TEntity, bool>> query)
    {
        return await _entitySet.Where(query).ToListAsync();
    }

    public void DeleteEntity(TEntity entity)
    {
        _entitySet.Remove(entity);
    }

    public void DeleteAllEntities()
    {
        _entitySet.RemoveRange(_entitySet);
    }

    public void UpdateEntity(TEntity outdatedEntity, string entityJson)
    {
        outdatedEntity.UpdateEntityByReflection(entityJson);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}