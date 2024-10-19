using Internship.AuthorizationAuthentication.Api.Core.Models.Input;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;

namespace Internship.AuthorizationAuthentication.Api.Presentation.Controllers;

[ApiController]
[WithClaimsAuthorization("Admin")]
[Route("student-register-requests")]
public class StudentRegisterRequestController : ControllerBase
{
    private readonly IStudentRegisterRequestService _studentRegisterRequestService;

    public StudentRegisterRequestController(IStudentRegisterRequestService studentRegisterRequestService)
    {
        _studentRegisterRequestService = studentRegisterRequestService;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentRegisterRequest>> GetRegisterRequestByIdAsync([FromRoute] int id)
    {
        return Ok(await _studentRegisterRequestService.GetRegisterRequestByIdAsync(id));
    }

    [HttpGet]
    public async Task<ActionResult<List<StudentRegisterRequest>>> GetAllRegisterRequestsAsync()
    {
        return Ok(await _studentRegisterRequestService.GetAllRegisterRequests());
    }
    
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<StudentRegisterRequest>>> GetOrderedRegisterRequestsAsync([FromBody] PaginationSetting paginationSetting)
    {
        return Ok(await _studentRegisterRequestService.GetOrderedRegisterRequestsAsync(paginationSetting));
    }

    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<StudentRegisterRequest>>> GetFilteredRegisterRequestsAsync([FromBody] FilteringSettings filteringSettings)
    {
        return Ok(await _studentRegisterRequestService.GetFilteredRegisterRequestsAsync(filteringSettings));
    }
    
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<StudentRegisterRequest>>> GetFilteredAndOrderedRegisterRequestsAsync([FromBody] FilterOrderSettings settings)
    {
        return Ok(await _studentRegisterRequestService.GetFilteredAndOrderedRegisterRequestsAsync(settings));
    }
    
    [AllowAnonymous]
    [HttpPost("register-request")]
    public async Task<ActionResult> AddRegisterRequestAsync([FromBody] StudentRegisterRequest studentRegisterRequest)
    {
        await _studentRegisterRequestService.AddRegisterRequestAsync(studentRegisterRequest);
        return Ok("Successfully added a new register request!");
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRegisterRequestById([FromRoute] int id)
    {
        await _studentRegisterRequestService.DeleteRegisterRequestByIdAsync(id);
        return NoContent();
    }
}