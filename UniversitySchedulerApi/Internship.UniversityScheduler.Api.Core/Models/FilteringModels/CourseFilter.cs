using Internship.UniversityScheduler.Library.SharedEnums;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models.FilteringModels;

[Registration(Type = RegistrationKind.Scoped)]
public class CourseFilter : IFilter<Course>
{
    public IQueryable<Course> Filter(IQueryable<Course> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(Course).GetProperties().FirstOrDefault(property =>
                string.Equals(propertyName, property.Name, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }

            switch (propertyInfo.Name)
            {
                case "CourseName":
                    filterQuery = filterQuery.Where(course => EF.Functions.Like(course.CourseName, propertyValue) || course.CourseName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "Id":
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(course => course.Id == idPropertyValue);
                    break;

                case "ProfessorId":
                    int.TryParse(propertyValue, out var professorIdPropertyValue);
                    filterQuery = filterQuery.Where(course => course.ProfessorId == professorIdPropertyValue);
                    break;

                case "NumberOfCredits":
                    int.TryParse(propertyValue, out var numberOfCreditsPropertyValue);
                    filterQuery = filterQuery.Where(course => course.NumberOfCredits == numberOfCreditsPropertyValue);
                    break;

                case "Type":
                    Enum.TryParse<CourseType>(propertyValue, out var courseTypePropertyValue);
                    filterQuery = filterQuery.Where(course => course.Type == courseTypePropertyValue);
                    break;

                case "Domain":
                    Enum.TryParse<CourseDomain>(propertyValue, out var courseDomainPropertyValue);
                    filterQuery = filterQuery.Where(course => course.Domain == courseDomainPropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}