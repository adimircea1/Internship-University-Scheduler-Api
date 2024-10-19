using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models.FilteringModels;

[Registration(Type = RegistrationKind.Scoped)]
public class AttendanceFilter : IFilter<Attendance>
{
    public IQueryable<Attendance> Filter(IQueryable<Attendance> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(Attendance).GetProperties().FirstOrDefault(property =>
                string.Equals(propertyName, property.Name, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }

            switch (propertyInfo.Name)
            {
                case "Id":
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(attendance => attendance.Id == idPropertyValue);
                    break;

                case "StudentId":
                    int.TryParse(propertyValue, out var studentIdPropertyValue);
                    filterQuery = filterQuery.Where(attendance => attendance.StudentId == studentIdPropertyValue);
                    break;

                case "CourseId":
                    int.TryParse(propertyValue, out var courseIdPropertyValue);
                    filterQuery = filterQuery.Where(attendance => attendance.CourseId == courseIdPropertyValue);
                    break;

                case "DateOfTheCourse":
                    DateOnly.TryParse(propertyValue, out var dateOnlyPropertyValue);
                    filterQuery = filterQuery.Where(attendance => attendance.DateOfTheCourse == dateOnlyPropertyValue);
                    break;

                case "TimeOfTheCourse":
                    TimeOnly.TryParse(propertyValue, out var timeOnlyPropertyValue);
                    filterQuery = filterQuery.Where(attendance => attendance.TimeOfTheCourse == timeOnlyPropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}