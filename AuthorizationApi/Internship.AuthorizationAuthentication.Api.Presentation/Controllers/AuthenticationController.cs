using System.Security.Claims;
using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internship.AuthorizationAuthentication.Api.Presentation.Controllers;

[ApiController]
[Route("authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IUserAuthenticationService _userAuthenticationService;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IUserAuthenticationService userAuthenticationService)
    {
        _logger = logger;
        _userAuthenticationService = userAuthenticationService;
    }

    [HttpGet("student/register/{registerId:int}")]
    public async Task<ActionResult<string>> RegisterStudentUserAsync([FromRoute] int registerId)
    {
        var generatedEmail = await _userAuthenticationService.RegisterStudentUserAsync(registerId);
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully registered a new student user!");
        return Ok(generatedEmail);
    }
    
    [HttpGet("professor/register/{registerId:int}")]
    public async Task<ActionResult<string>> RegisterProfessorUserAsync([FromRoute] int registerId)
    {
        var generatedEmail = await _userAuthenticationService.RegisterProfessorUserAsync(registerId);
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully registered a new professor user!");
        return Ok(generatedEmail);
    }


    [HttpPost("login")]
    public async Task<ActionResult<string>> LoginAsync([FromBody] LoginRequest request)
    {
        var response = await _userAuthenticationService.LoginUserAsync(request);
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully retrieved the user access and refresh token!");
        return response;
    }

    //this is a way for the user to "invalidate" the refresh tokens
    [Authorize]
    [HttpDelete("logout")]
    public async Task<ActionResult> LogoutAsync()
    {
        var userId = HttpContext.User.FindFirstValue("Id");
        await _userAuthenticationService.LogoutUserAsync(userId);
        _logger.LogInformation($"\n{DateTime.Now} ---> Successfully deleted a token!");
        return NoContent();
    }
}