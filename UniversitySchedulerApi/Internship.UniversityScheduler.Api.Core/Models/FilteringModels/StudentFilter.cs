using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models.FilteringModels;

[Registration(Type = RegistrationKind.Scoped)]
public class StudentFilter : IFilter<Student>
{
    public IQueryable<Student> Filter(IQueryable<Student> filterQuery, Dictionary<string,string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(Student).GetProperties().FirstOrDefault(property => 
                string.Equals(property.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }
            
            switch (propertyInfo.Name)
            {
                case "FirstName" :
                    filterQuery = filterQuery.Where(student => EF.Functions.Like(student.FirstName, propertyValue) || student.FirstName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "LastName" :
                    filterQuery = filterQuery.Where(student => EF.Functions.Like(student.LastName, propertyValue) || student.LastName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "FullName" :
                    filterQuery = filterQuery.Where(student => EF.Functions.Like(student.LastName, propertyValue) || student.FullName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "Email" :
                    filterQuery = filterQuery.Where(student => EF.Functions.Like(student.Email, propertyValue) || student.Email.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "PersonalEmail" :
                    filterQuery = filterQuery.Where(student => EF.Functions.Like(student.PersonalEmail, propertyValue) || student.PersonalEmail.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "Id" :
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(student => student.Id == idPropertyValue);
                    break;
                
                case "StudyYear" :
                    int.TryParse(propertyValue, out var studyYearPropertyValue);
                    filterQuery = filterQuery.Where(student => student.StudyYear == studyYearPropertyValue);
                    break;
                
                case "UniversityGroupId" :
                    if (propertyValue == "null")
                    {
                        filterQuery = filterQuery.Where(student => student.UniversityGroupId == null);
                        break;
                    }
                    
                    int.TryParse(propertyValue, out var universityGroupIdPropertyValue);
                    filterQuery = filterQuery.Where(student => student.UniversityGroupId == universityGroupIdPropertyValue);
                    break;
                    
                case "PhoneNumber" : 
                    filterQuery = filterQuery.Where(student => student.PhoneNumber == propertyValue);
                    break;
                
                case "BirthdayDate" :
                    DateOnly.TryParse(propertyValue, out var dateOnlyPropertyValue);
                    filterQuery = filterQuery.Where(student => student.BirthdayDate == dateOnlyPropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}