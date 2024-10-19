using Internship.UniversityScheduler.Library.SharedEnums;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models.FilteringModels;

[Registration(Type = RegistrationKind.Scoped)]
public class UniversityGroupFilter : IFilter<UniversityGroup>
{
    public IQueryable<UniversityGroup> Filter(IQueryable<UniversityGroup> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(UniversityGroup).GetProperties().FirstOrDefault(property =>
                string.Equals(propertyName, property.Name, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }

            switch (propertyInfo.Name)
            {
               
                case "Id":
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(group => group.Id == idPropertyValue);
                    break;
                
                case "NumberOfMembers":
                    int.TryParse(propertyValue, out var numberOfMembersPropertyValue);
                    filterQuery = filterQuery.Where(group => group.NumberOfMembers == numberOfMembersPropertyValue);
                    break;
                
                case "MaxSize":
                    int.TryParse(propertyValue, out var maxSizePropertyValue);
                    filterQuery = filterQuery.Where(group => group.MaxSize == maxSizePropertyValue);
                    break;
               
                case "Name":
                    filterQuery = filterQuery.Where(group => EF.Functions.Like(group.Name, propertyValue) || group.Name.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "Type":
                    Enum.TryParse<UniversitySpecialization>(propertyValue, out var universityGroupSpecializationPropertyValue);
                    filterQuery = filterQuery.Where(group => group.Specialization == universityGroupSpecializationPropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}