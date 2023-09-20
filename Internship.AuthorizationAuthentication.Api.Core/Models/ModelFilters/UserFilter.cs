using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Models.Enums;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.ModelFilters;

[Registration(Type = RegistrationKind.Scoped)]
public class UserFilter : IFilter<User>
{
    public IQueryable<User> Filter(IQueryable<User> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(User).GetProperties().FirstOrDefault(property => string.Equals(property.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }
            
            switch (propertyInfo.Name)
            {
                case "UserName":
                    filterQuery = filterQuery.Where(user => EF.Functions.Like(user.UserName, propertyValue) || user.UserName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "Email":
                    filterQuery = filterQuery.Where(user => EF.Functions.Like(user.Email, propertyValue) || user.UserName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;
                
                case "Id":
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(user => user.Id == idPropertyValue);
                    break;
                    
                case "Role":
                    Enum.TryParse<UserType>(propertyValue, out var userRolePropertyValue);
                    filterQuery = filterQuery.Where(user => user.Role == userRolePropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}