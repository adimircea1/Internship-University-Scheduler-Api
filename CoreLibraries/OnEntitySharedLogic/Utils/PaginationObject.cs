using System.Linq.Expressions;
using OnEntitySharedLogic.Models;

namespace OnEntitySharedLogic.Utils;

public class PaginationObject<TEntity> where TEntity : IEntity
{
    public int Skip { get; set; }
    public int Take { get; set; } 
    public Expression<Func<TEntity, object>> OrderBy { get; set; } = null!;
    public OrderByDirection OrderDirection { get; set; } 
}