using Microsoft.AspNetCore.Http;

namespace OnEntitySharedLogic.Auth;

public class UserAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDistributedCacheService _distributedCacheService;

    public UserAuthenticationMiddleware(RequestDelegate next, IDistributedCacheService distributedCacheService)
    {
        _next = next;
        _distributedCacheService = distributedCacheService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
        
        

        await _next(context);
    }
}