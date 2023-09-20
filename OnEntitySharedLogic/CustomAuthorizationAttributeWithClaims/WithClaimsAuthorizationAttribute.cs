using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;

//This allows authorization on custom Roles -> If the JWT access token is not a valid token, i can't get user claims (role) from it
public class WithClaimsAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _allowedEntities;
    
    public WithClaimsAuthorizationAttribute(params string[] allowedEntities)
    {
        _allowedEntities = allowedEntities;
    }

    public void OnAuthorization(AuthorizationFilterContext context) //context provides information about current HTTP request
    {
        //ActionDescriptor => description of the current executed action
        //EndpointMetadata => data about the current action
        
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.Any(metaData => metaData is AllowAnonymousAttribute);
        if (allowAnonymous)
        {
            return;
        }
        
        var userClaims = context.HttpContext.User.Claims;
        var roleUserClaim = userClaims.FirstOrDefault(claim => claim.Type == nameof(ClaimTypes.Role));
        
        var userIdentity = context.HttpContext.User.Identity;

        if (userIdentity is null || !userIdentity.IsAuthenticated || roleUserClaim is null)  
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (_allowedEntities.All(entity => entity != roleUserClaim.Value))
        {
            context.Result = new ForbidResult();
        }
    }
}