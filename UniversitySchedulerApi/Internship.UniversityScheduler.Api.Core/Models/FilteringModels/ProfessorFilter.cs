using Internship.UniversityScheduler.Library.SharedEnums;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models.FilteringModels;

[Registration(Type = RegistrationKind.Scoped)]
public class ProfessorFilter : IFilter<Professor>
{
    public IQueryable<Professor> Filter(IQueryable<Professor> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(Professor).GetProperties().FirstOrDefault(property =>
                string.Equals(property.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }

            switch (propertyInfo.Name)
            {
                case "FirstName":
                    filterQuery = filterQuery.Where(professor => EF.Functions.Like(professor.FirstName, propertyValue) || professor.FirstName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "LastName":
                    filterQuery = filterQuery.Where(professor => EF.Functions.Like(professor.LastName, propertyValue) || professor.LastName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "Email":
                    filterQuery = filterQuery.Where(professor => EF.Functions.Like(professor.Email, propertyValue) || professor.Email.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "PhoneNumber":
                    filterQuery = filterQuery.Where(professor => professor.PhoneNumber == propertyValue);
                    break;

                case "BirthdayDate":
                    DateOnly.TryParse(propertyValue, out var dateOnlyPropertyValue);
                    filterQuery = filterQuery.Where(professor => professor.BirthdayDate == dateOnlyPropertyValue);
                    break;

                case "Speciality":
                    Enum.TryParse<ProfessorSpeciality>(propertyValue, out var professorSpecialityValue);
                    filterQuery = filterQuery.Where(professor => professor.Speciality == professorSpecialityValue);
                    break;

                case "Id":
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(professor => professor.Id == idPropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}