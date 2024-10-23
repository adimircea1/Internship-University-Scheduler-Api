namespace OnEntitySharedLogic.Utils;

public class FilterObject<TEntity> where TEntity : IEntity
{
    public IFilter<TEntity> EntityFilter { get; set; } = null!;
    public Dictionary<string, string> FilterByProperties { get; set; } = null!;
}
