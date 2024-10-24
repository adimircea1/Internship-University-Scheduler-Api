using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using OnEntitySharedLogic.Auth;

namespace Internship.AuthorizationAuthentication.Library.Auth;

public class UserAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCacheService _distributedCacheService;
    private readonly IUserService _userService;
    
    public UserAuthenticationMiddleware(RequestDelegate next,
        IDistributedCacheService distributedCacheService,
        IUserService userService)
    {
        _next = next;
        _distributedCacheService = distributedCacheService;
        _userService = userService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is null|| !context.User.Identity.IsAuthenticated)
        {
            await SetUnauthorizedAccessAsync(context);
            return;
        }
        
        var claims = context.User.Claims.ToList();

        if (claims is null || !claims.Any())
        {
            await SetUnauthorizedAccessAsync(context);
            return;
        }

        var id = claims.FirstOrDefault(claim => claim.Type == "Id");

        if (id is null)
        {
            await SetUnauthorizedAccessAsync(context);
            return;
        }

        var existingUser = await _userService.GetUserByIdAsync(int.Parse(id.Value));

        if (existingUser is null)
        {
            await SetUnauthorizedAccessAsync(context);
            return;
        }
        
        var key = $"{existingUser.Email}_AccessToken_{existingUser.Id}";
        var accessToken = await _distributedCacheService.GetAsync<string>(key);

        if (accessToken != context.Request.Headers["Authorization"])
        {
            await SetUnauthorizedAccessAsync(context);
            return;
        }

        await _next(context);
    }

    private static async Task SetUnauthorizedAccessAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
    }
}