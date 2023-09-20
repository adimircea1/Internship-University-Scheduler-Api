using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Moq;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Utils;

namespace CoreLogicTesting;

public class OnGenericDatabaseRepositoryTesting
{
    public class EntityToTest : IEntity
    {
        public int Id { get; set; }
        public int? IntValue { get; set; }
        public int? SecondIntValue { get; set; }
        public string? StringValue { get; set; }
    }

    public class DataContext : DbContext, IDataContext
    {
        public DbSet<EntityToTest>? EntitiesToTest { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }

    private readonly List<EntityToTest> _entities;
    private readonly IDatabaseGenericRepository<EntityToTest> _entityRepository;

    public OnGenericDatabaseRepositoryTesting()
    {
        _entities = new Fixture().CreateMany<EntityToTest>(3).ToList();

        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var dbContext = new DataContext(options);

        var mockFilter = new Mock<IFilter<EntityToTest>>();
        
        _entityRepository = new DatabaseGenericRepository<EntityToTest>(dbContext);
    }

    [Fact]
    public async Task AddEntityAsync_Adds_Entity_In_Database_Async()
    {
        var entityToAdd = _entities.First();
        await _entityRepository.AddEntityAsync(entityToAdd);
        await _entityRepository.SaveChangesAsync();
    }

    [Fact]
    public async Task GetEntityByQuery_Returns_Entity_By_Query_If_Existent_Or_Null_If_Not_Async()
    {
        var entityToAdd = _entities.First();
        await _entityRepository.AddEntityAsync(entityToAdd);
        await _entityRepository.SaveChangesAsync();

        var existingEntity = await _entityRepository.GetEntityByQueryAsync(entity => entity.Id == entityToAdd.Id);
        Assert.NotNull(existingEntity);

        var nonExistentEntity =
            await _entityRepository.GetEntityByQueryAsync(entity => entity.StringValue == "Alabala");
        Assert.Null(nonExistentEntity);
    }

    [Fact]
    public async Task DeleteEntityById_Deletes_Entity_From_Database_Ok()
    {
        var entityToAdd = _entities.First();
        await _entityRepository.AddEntityAsync(entityToAdd);
        await _entityRepository.SaveChangesAsync();

        var existingEntity = await _entityRepository.GetEntityByQueryAsync(entity => entity.Id == entityToAdd.Id);
        Assert.NotNull(existingEntity);

        _entityRepository.DeleteEntity(existingEntity);
        await _entityRepository.SaveChangesAsync();

        existingEntity = await _entityRepository.GetEntityByQueryAsync(entity => entity.Id == entityToAdd.Id);
        Assert.Null(existingEntity);
    }

    [Fact]
    public async Task DeleteAllEntities_Deletes_All_Entities_From_Database_Ok()
    {
        foreach (var entity in _entities)
        {
            await _entityRepository.AddEntityAsync(entity);
        }

        await _entityRepository.SaveChangesAsync();
        var numberOfEntitiesFromDatabase = (await _entityRepository.GetAllEntitiesAsync()).Count;
        
        Assert.NotEqual(0, numberOfEntitiesFromDatabase);

        _entityRepository.DeleteAllEntities();
        await _entityRepository.SaveChangesAsync();
        numberOfEntitiesFromDatabase = (await _entityRepository.GetAllEntitiesAsync()).Count;
        
        Assert.Equal(0, numberOfEntitiesFromDatabase);
    }
}