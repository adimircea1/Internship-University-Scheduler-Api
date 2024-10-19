using Microsoft.EntityFrameworkCore;

namespace OnEntitySharedLogic.DatabaseGenericRepository;

public interface IDataContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync();
}

