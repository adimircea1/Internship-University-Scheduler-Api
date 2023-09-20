using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Internship.UniversityScheduler.Api.Infrastructure.Utils;

public class TimeOnlyToStringConverter : ValueConverter<TimeOnly, string>
{
    public TimeOnlyToStringConverter() : base(time => time.ToString("HH:mm:ss"),
        content => StringToTimeOnly(content))
    {
    }

    private static TimeOnly StringToTimeOnly(string content)
    {
        TimeSpan.TryParse(content, out var timeSpan);
        return TimeOnly.FromTimeSpan(timeSpan);
    }
}