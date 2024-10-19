using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Microsoft.EntityFrameworkCore;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.ModelFilters;

[Registration(Type = RegistrationKind.Scoped)]
public class StudentRegisterRequestFilter : IFilter<StudentRegisterRequest>
{
    public IQueryable<StudentRegisterRequest> Filter(IQueryable<StudentRegisterRequest> filterQuery, Dictionary<string, string> filterBy)
    {
        foreach (var (propertyName, propertyValue) in filterBy)
        {
            var propertyInfo = typeof(StudentRegisterRequest).GetProperties().FirstOrDefault(property => string.Equals(property.Name, propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo is null)
            {
                continue;
            }

            switch (propertyInfo.Name)
            {
                case "FirstName":
                    filterQuery = filterQuery.Where(request => EF.Functions.Like(request.FirstName, propertyValue) || request.FirstName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "LastName":
                    filterQuery = filterQuery.Where(request => EF.Functions.Like(request.LastName, propertyValue) || request.LastName.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "Email":
                    filterQuery = filterQuery.Where(request => EF.Functions.Like(request.Email, propertyValue) || request.Email.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "Id":
                    int.TryParse(propertyValue, out var idPropertyValue);
                    filterQuery = filterQuery.Where(request => request.Id == idPropertyValue);
                    break;

                case "PhoneNumber":
                    filterQuery = filterQuery.Where(request => EF.Functions.Like(request.PhoneNumber, propertyValue) || request.PhoneNumber.ToLower().StartsWith(propertyValue.ToLower()));
                    break;

                case "Birthdate":
                    DateOnly.TryParse(propertyValue, out var birthdatePropertyValue);
                    filterQuery = filterQuery.Where(request => request.Birthdate == birthdatePropertyValue);
                    break;
            }
        }

        return filterQuery;
    }
}