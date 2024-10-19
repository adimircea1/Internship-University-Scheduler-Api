using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Internship.UniversityScheduler.Api.Infrastructure.Utils;

public class DateOnlyToStringConverter : ValueConverter<DateOnly, string>
{
    public DateOnlyToStringConverter() : base(date => date.ToString(),
        content => StringToDateOnly(content))
    {
    }

    private static DateOnly StringToDateOnly(string content)
    {
        DateTime.TryParse(content, out var dateTime);

        var dateOnly = DateOnly.FromDateTime(dateTime);

        return dateOnly;
    }
}