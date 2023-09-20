using System.ComponentModel.DataAnnotations;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.Utils;

namespace CoreLogicTesting;

public class OnValidationTesting
{
    private class EntityToTest : IEntity
    {
        [Validate(0)]
        public int Id { get; set; }
        
        [Validate(null, 50)]
        public int? IntValue { get; set; }
        
        [Validate(10, 10)]
        public int? SecondIntValue { get; set; }
        
        [Validate(5)]
        public string? StringValue { get; set; }

        [Validate(5)]
        public string SecondStringValue { get; set; } = string.Empty;
        
        [Validate("08:00:00", "17:00:00")]
        public TimeOnly? TimeOnlyValue { get; set; }
    }

    [Fact]
    public void Validating_An_Entity_With_IntValue_Bigger_Than_50_Throws_Validation_Exception()
    {
        var testEntity = new EntityToTest
        {
            IntValue = 51,
        };

        Assert.Throws<ValidationException>(() => testEntity.ValidateEntity());
    }
    
    [Theory]
    [InlineData(11)]
    [InlineData(-10)]
    [InlineData(50)]
    public void Validating_An_Entity_With_IntValue_Smaller_Or_Equal_Than_50_Ok(int value)
    {
        var testEntity = new EntityToTest
        {
            IntValue = value,
        };

        testEntity.ValidateEntity();
    }
    

    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(-10)]
    public void Validating_An_Entity_With_SecondIntValue_Not_10_Throws_Validation_Exception(int value)
    {
        var testEntity = new EntityToTest
        {
            SecondIntValue = value,
        }; 
        
        Assert.Throws<ValidationException>(() => testEntity.ValidateEntity());
    }

    [Fact]
    public void Validating_An_Entity_With_SecondIntValue_10_Ok()
    {
        var testEntity = new EntityToTest
        {
            SecondIntValue = 10
        }; 
        
        testEntity.ValidateEntity();
    }

    [Fact]
    public void Validating_An_Entity_With_Id_Negative_Throws_Validation_Exception()
    {
        var testEntity = new EntityToTest
        {
            Id = -10
        };
        
        Assert.Throws<ValidationException>(() => testEntity.ValidateEntity());
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Validating_An_Entity_With_Id_Bigger_Or_Equal_Than_0_Is_Ok(int value)
    {
        var testEntity = new EntityToTest
        {
            Id = value
        };
        
        testEntity.ValidateEntity();
    }

    [Fact]
    public void Validating_An_Entity_With_StringValue_Length_Smaller_Than_5_Throws_Validation_Exception()
    {
        var testEntity = new EntityToTest
        {
            StringValue = "1234"
        };
        
        Assert.Throws<ValidationException>(() => testEntity.ValidateEntity());
    }

    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(18, 0 ,0)]
    [InlineData(7, 59, 59)]
    [InlineData(17, 0, 1)]

    public void Validating_An_Entity_With_TimeOnlyValue_Not_Between_8AM_5PM_Throws_Validation_Exception(int hours, int minutes, int seconds)
    {
        var testEntity = new EntityToTest
        {
            TimeOnlyValue = new TimeOnly(hours, minutes,seconds)
        };
        
        Assert.Throws<ValidationException>(() => testEntity.ValidateEntity());
    }
    
    [Theory]
    [InlineData(8, 0, 0)]
    [InlineData(8, 0, 1)]
    [InlineData(8, 1, 0)]
    [InlineData(13, 0 ,0)]
    [InlineData(14, 23 ,30)]
    public void Validating_An_Entity_With_TimeOnlyValue_Between_8AM_5PM_Is_Ok(int hours, int minutes, int seconds)
    {
        var testEntity = new EntityToTest
        {
            TimeOnlyValue = new TimeOnly(hours, minutes,seconds)
        };

        testEntity.ValidateEntity();
    }
}