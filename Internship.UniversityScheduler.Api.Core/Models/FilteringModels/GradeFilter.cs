using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models.FilteringModels;

[Registration(Type = RegistrationKind.Scoped)]
public class GradeFilter : IFilter<Grade>
{
    public string? FilterByCourseDomain { get; set; }

    public IQueryable<Grade> Filter(IQueryable<Grade> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(Grade).GetProperties().FirstOrDefault(property =>
                string.Equals(propertyName, property.Name, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }

            switch (propertyInfo.Name)
            {
                case "Id":
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(grade => grade.Id == idPropertyValue);
                    break;

                case "StudentId":
                    int.TryParse(propertyValue, out var studentIdPropertyValue);
                    filterQuery = filterQuery.Where(grade => grade.StudentId == studentIdPropertyValue);
                    break;

                case "CourseId":
                    int.TryParse(propertyValue, out var courseIdPropertyValue);
                    filterQuery = filterQuery.Where(grade => grade.CourseId == courseIdPropertyValue);
                    break;

                case "CatalogueId":
                    int.TryParse(propertyValue, out var catalogueIdPropertyValue);
                    filterQuery = filterQuery.Where(grade => grade.CatalogueId == catalogueIdPropertyValue);
                    break;

                case "Value":
                    int.TryParse(propertyValue, out var gradePropertyValue);
                    filterQuery = filterQuery.Where(grade => grade.Value == gradePropertyValue);
                    break;
                
                case "DateOfGrade":
                    DateTime.TryParse(propertyValue, out var dateTimePropertyValue);
                    filterQuery = filterQuery.Where(grade => grade.DateOfGrade == dateTimePropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}