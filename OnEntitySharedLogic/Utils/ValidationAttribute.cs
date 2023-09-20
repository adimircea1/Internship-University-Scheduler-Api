namespace OnEntitySharedLogic.Utils;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ValidateAttribute : Attribute
{
    public ValidateAttribute(object? minValue = null, object? maxValue = null)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    private object? MinValue { get; }
    private object? MaxValue { get; }

    public bool Validate(object propertyValue)
    {
        switch (MinValue, MaxValue)
        {
            case { MinValue: not null, MaxValue: not null }:
                if (CompareValues(propertyValue, MinValue) < 0 || 
                    CompareValues(propertyValue, MaxValue) > 0)
                {
                    return false;
                }
                break;

            case { MinValue: not null, MaxValue: null }:
                if (CompareValues(propertyValue, MinValue) < 0)
                {
                    return false;
                }
                break;

            case { MinValue: null, MaxValue: not null }:
                if (CompareValues(propertyValue, MaxValue) > 0)
                {
                    return false;
                }
                break;

            case { MinValue: null, MaxValue: null }:
                break;
        }

        return true;
    }

    private static int CompareValues(object value, object compareTo)
    {
        if (value is int intValue && compareTo is int intCompareTo)
        {
            return intValue.CompareTo(intCompareTo);
        }

        if (value is string stringValue && compareTo is int intValueCompareTo)
        {
            return stringValue.Length.CompareTo(intValueCompareTo);
        }

        if (value is TimeOnly timeOnlyValue && compareTo is string stringValueCompareTo)
        {
            TimeOnly.TryParse(stringValueCompareTo, out var timeOnlyValueCompareTo);
            return timeOnlyValue.CompareTo(timeOnlyValueCompareTo);
        }
        
        throw new InvalidCastException(
            $"Cannot compare values of type '{value.GetType().Name}' and '{compareTo.GetType().Name}'!");
    }
}