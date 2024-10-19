using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;
using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Models.ModelFilters;

[Registration(Type = RegistrationKind.Scoped)]
public class ExamFilter : IFilter<Exam>
{
    public IQueryable<Exam> Filter(IQueryable<Exam> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            switch (propertyName.ToLower())
            {
                case "available":
                {
                    var now = DateTime.UtcNow;
                    filterQuery = filterQuery.Where(exam => exam.AvailableFrom.ToUniversalTime() <= now && exam.AvailableUntil.ToUniversalTime() >= now && exam.FinalGrade == null);
                    break;
                }
                case "unavailable":
                {
                    var now = DateTime.UtcNow;
                    filterQuery =  filterQuery.Where(exam => exam.AvailableFrom.ToUniversalTime() > now || exam.AvailableUntil.ToUniversalTime() < now || exam.FinalGrade != null);

                    break;
                }
            }
            
            var propertyInfo = typeof(Exam).GetProperties().FirstOrDefault(property => 
                string.Equals(property.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }
            
            switch (propertyInfo.Name)
            {
                case "Id" :
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(exam => exam.Id == idPropertyValue);
                    break;
                
                case "CourseId" :
                    int.TryParse(propertyValue, out var courseIdPropertyValue);
                    filterQuery = filterQuery.Where(exam => exam.CourseId == courseIdPropertyValue);
                    break;
               
                case "FinalGrade" : 
                    int.TryParse(propertyValue, out var finalGradePropertyValue);
                    filterQuery = filterQuery.Where(exam => exam.FinalGrade == finalGradePropertyValue);
                    break;
                
                case "AvailableFrom":
                    DateTime.TryParse(propertyValue, out var availableFromPropertyValue);
                    filterQuery = filterQuery.Where(exam => exam.AvailableFrom >= availableFromPropertyValue);
                    break;

                case "AvailableUntil":
                    DateTime.TryParse(propertyValue, out var availableUntilPropertyValue);
                    filterQuery = filterQuery.Where(exam => exam.AvailableUntil <= availableUntilPropertyValue);
                    break;

                case "PartialGradingAllowed":
                    bool.TryParse(propertyValue, out var partialGradingAllowedPropertyValue);
                    filterQuery = filterQuery.Where(exam => exam.PartialGradingAllowed == partialGradingAllowedPropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}