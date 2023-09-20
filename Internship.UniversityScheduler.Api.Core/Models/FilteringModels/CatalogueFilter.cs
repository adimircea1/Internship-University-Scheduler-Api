using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models.FilteringModels;

[Registration(Type = RegistrationKind.Scoped)]
public class CatalogueFilter : IFilter<Catalogue>
{
    public IQueryable<Catalogue> Filter(IQueryable<Catalogue> filterQuery, Dictionary<string,string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(Catalogue).GetProperties().FirstOrDefault(property => 
                string.Equals(property.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }

            switch (propertyInfo.Name)
            {
                case "UniversityGroupId":
                    int.TryParse(propertyValue, out var universityGroupIdPropertyValue);
                    filterQuery = filterQuery.Where(catalogue => catalogue.Id == universityGroupIdPropertyValue);
                    break;
                
                case "Id" :
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(catalogue => catalogue.Id == idPropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}