using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace OnEntitySharedLogic.Extensions;

public static class HttpExtension
{
    public static string? GetAccessToken(this IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available in the current context!");
        var accessToken = httpContext.Request.Headers.Authorization.FirstOrDefault();

        return accessToken;
    }
    
    public static int? GetUserIdClaim(this IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available in the current context!");
        var idClaimToString = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Id")!.ToString();

        if (string.IsNullOrEmpty(idClaimToString))
        {
            return null;
        }

        var match = Regex.Match(idClaimToString, @"Id:\s*(\d+)");
        if (match.Success && int.TryParse(match.Groups[1].Value, out var userId))
        {
            return userId;
        }

        return null;
    }

    public static string? GetUserClaimRole(this IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available in the current context!");
        var userRole = httpContext.User.Claims.FirstOrDefault(claim => claim.Type == "Role")!.ToString();

        return string.IsNullOrEmpty(userRole) ? null : userRole;
    }

    public static void AddAccessTokenBearerScheme(this HttpClient httpClient, string accessToken)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken["Bearer ".Length..].Trim());
    }
}