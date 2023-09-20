using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;

namespace Internship.AuthorizationAuthentication.Api.Presentation.Controllers;

[ApiController]
[Route("refresh-tokens")]
public class RefreshTokenController : ControllerBase
{
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenController(IRefreshTokenService refreshTokenService){
        _refreshTokenService = refreshTokenService;
    }

    [AllowAnonymous]
    [HttpDelete]
    public async Task<ActionResult> RemoveRefreshTokenByValueAsync([FromBody] string refreshToken)
    {
        await _refreshTokenService.DeleteRefreshTokenByValue(refreshToken);
        return NoContent();
    }
    
    //[WithClaimsAuthorization("Admin")]
    [HttpDelete("user/{userId:int}")]
    public async Task<ActionResult> RemoveRefreshTokenByUserId([FromRoute] int userId)
    {
        await _refreshTokenService.DeleteRefreshTokenByUserIdAsync(userId);
        return NoContent();
    }
    
}