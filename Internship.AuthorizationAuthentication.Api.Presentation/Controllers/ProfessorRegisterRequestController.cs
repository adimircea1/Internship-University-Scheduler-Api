using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;

namespace Internship.AuthorizationAuthentication.Api.Presentation.Controllers;

[ApiController]
[WithClaimsAuthorization("Admin")]
[Route("professor-register-requests")]
public class ProfessorRegisterRequestController : ControllerBase
{
    private readonly IProfessorRegisterRequestService _professorRegisterRequestService;

    public ProfessorRegisterRequestController(IProfessorRegisterRequestService professorRegisterRequestService)
    {
        _professorRegisterRequestService = professorRegisterRequestService;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfessorRegisterRequest>> GetRegisterRequestByIdAsync([FromRoute] int id)
    {
        return Ok(await _professorRegisterRequestService.GetRegisterRequestByIdAsync(id));
    }

    [HttpGet]
    public async Task<ActionResult<List<ProfessorRegisterRequest>>> GetAllRegisterRequestsAsync()
    {
        return Ok(await _professorRegisterRequestService.GetAllRegisterRequests());
    }
    
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<ProfessorRegisterRequest>>> GetOrderedRegisterRequestsAsync([FromBody] PaginationSetting paginationSetting)
    {
        return Ok(await _professorRegisterRequestService.GetOrderedRegisterRequestsAsync(paginationSetting));
    }

    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<ProfessorRegisterRequest>>> GetFilteredRegisterRequestsAsync([FromBody] FilteringSettings filteringSettings)
    {
        return Ok(await _professorRegisterRequestService.GetFilteredRegisterRequestsAsync(filteringSettings));
    }
    
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<ProfessorRegisterRequest>>> GetFilteredAndOrderedRegisterRequestsAsync([FromBody] FilterOrderSettings settings)
    {
        return Ok(await _professorRegisterRequestService.GetFilteredAndOrderedRegisterRequestsAsync(settings));
    }
    
    [AllowAnonymous]
    [HttpPost("register-request")]
    public async Task<ActionResult> AddRegisterRequestAsync([FromBody] ProfessorRegisterRequest professorRegisterRequest)
    {
        await _professorRegisterRequestService.AddRegisterRequestAsync(professorRegisterRequest);
        return Ok("Successfully added a new register request!");
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRegisterRequestById([FromRoute] int id)
    {
        await _professorRegisterRequestService.DeleteRegisterRequestByIdAsync(id);
        return NoContent();
    }
}