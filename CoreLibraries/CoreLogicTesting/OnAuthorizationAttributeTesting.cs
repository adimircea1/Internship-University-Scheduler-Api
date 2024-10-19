using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;

namespace CoreLogicTesting;

public class OnAuthorizationAttributeTesting
{
    private readonly WithClaimsAuthorizationAttribute _authorizationAttribute;
    private readonly AuthorizationFilterContext _context;

    public OnAuthorizationAttributeTesting()
    {
        _authorizationAttribute = new WithClaimsAuthorizationAttribute("Admin");

        var httpContext = new DefaultHttpContext();
        var routeData = new Microsoft.AspNetCore.Routing.RouteData();
        var actionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor();
        var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

        _context = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
    }

    [Fact]
    public void AuthorizationAttribute_Allows_Valid_User_With_Valid_Claims()
    {
        var claims = new[] { new Claim("Role", "Admin") };
        _context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));

        _authorizationAttribute.OnAuthorization(_context);
        Assert.Null(_context.Result);
    }

    [Fact]
    public void AuthorizationAttribute_ThrowsUnauthorized_For_Invalid_Or_Non_Existent_Claims()
    {
        var claims = new[] { new Claim("ROLE", "Admin") };
        _context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));

        _authorizationAttribute.OnAuthorization(_context);
        Assert.IsType<UnauthorizedResult>(_context.Result);
    }
    
    [Fact]
    public void AuthorizationAttribute_ThrowsForbidden_For_Role_Different_Than_The_Specified_Ones()
    {
        var claims = new[] { new Claim("Role", "Student") };
        _context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));

        _authorizationAttribute.OnAuthorization(_context);
        Assert.IsType<ForbidResult>(_context.Result);
    }
}