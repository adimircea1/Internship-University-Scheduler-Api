namespace OnEntitySharedLogic.Utils;
public interface IFilter<TEntity>
{
    public IQueryable<TEntity> Filter(IQueryable<TEntity> filterQuery, Dictionary<string, string> filterBy);
}