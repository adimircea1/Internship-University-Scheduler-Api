using Newtonsoft.Json;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Utils;

namespace CoreLogicTesting;

public class OnReflectionUpdateTesting
{
    private class EntityToTest : IEntity
    {
        public int Id { get; set; }
        public int? IntValue { get; set; }
        [Validate(0)]
        public int? SecondIntValue { get; set; }
        public string? StringValue { get; set; }
        public TimeOnly? TimeOnlyValue { get; set; }
        public DateOnly? DateOnlyValue { get; set; }
        public List<string> Strings { get; set; } = new();
    }

    [Fact]
    public void UpdateByReflection_Updates_Entity_Partially_By_Given_Properties()
    {
        var outdatedEntity = new EntityToTest
        {
            Id = 10,
            IntValue = 100,
            SecondIntValue = 1,
            StringValue = "abc",
            TimeOnlyValue = new TimeOnly( 15, 32, 10),
            DateOnlyValue = new DateOnly(2023, 6, 8),
            Strings = new List<string>
            {
                "abc",
                "daa"
            }
        };

        var updatedEntity = new 
        {
            IntValue = 200,
            StringValue = "abc",
            TimeOnlyValue = new TimeOnly(18, 40, 10),
            DateOnlyValue = new DateOnly(2024, 6, 8),
        };
        
        var jsonString = JsonConvert.SerializeObject(updatedEntity);
        outdatedEntity.UpdateEntityByReflection(jsonString);
        
        Assert.Equal( 10, outdatedEntity.Id);
        Assert.Equal(200, outdatedEntity.IntValue);
        Assert.Equal(1, outdatedEntity.SecondIntValue);
        Assert.Equal(new TimeOnly(18, 40, 10), outdatedEntity.TimeOnlyValue);
        Assert.Equal(new DateOnly(2024, 6, 8), outdatedEntity.DateOnlyValue);
    }

    [Fact]
    public void UpdateByReflection_Updates_Entity_List_Properties()
    {
        var outdatedEntity = new EntityToTest
        {
            Id = 10,
            IntValue = 100,
            SecondIntValue = 1,
            StringValue = "abc",
            TimeOnlyValue = new TimeOnly( 15, 32, 10),
            DateOnlyValue = new DateOnly(2023, 6, 8),
            Strings = new List<string>
            {
                "abc",
                "daa"
            }
        };

        var updatedEntity = new 
        {
            Strings = new List<string>
            {
                "1234",
                "1234567",
                "123194109"
            }
        };
        
        var jsonString = JsonConvert.SerializeObject(updatedEntity);
        outdatedEntity.UpdateEntityByReflection(jsonString);
        
        Assert.Equal(new List<string>
        {
            "1234",
            "1234567",
            "123194109"

        }, outdatedEntity.Strings);
    }
}