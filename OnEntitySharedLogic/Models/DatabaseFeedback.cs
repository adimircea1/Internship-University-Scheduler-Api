namespace OnEntitySharedLogic.Models;

public class DatabaseFeedback<TEntity>
{
    public List<TEntity> Entities { get; set; } = new();
    public int NumberOfEntities { get; set; }
}