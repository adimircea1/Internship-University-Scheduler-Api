using AutoMapper;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Presentation.Controllers;

[ApiController]
[Route("groups")]
public class UniversityGroupController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUniversityGroupService _universityGroupService;

    public UniversityGroupController(
        IUniversityGroupService universityGroupService,
        IMapper mapper)
    {
        _universityGroupService = universityGroupService;
        _mapper = mapper;
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost("group")]
    public async Task<ActionResult> AddUniversityGroupAsync([FromBody] UniversityGroupInputDto group)
    {
        await _universityGroupService.AddUniversityGroupAsync(_mapper.Map<UniversityGroup>(group));
        return Ok("Successfully added an university group!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost]
    public async Task<ActionResult> AddUniversityGroupsAsync([FromBody] List<UniversityGroupInputDto> groups)
    {
        await _universityGroupService.AddUniversityGroupsAsync(groups
            .Select(group => _mapper.Map<UniversityGroup>(group))
            .ToList());
        return Ok("Successfully added a list of university groups!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPatch("{groupId:int}/student/{studentId:int}")]
    public async Task<ActionResult> AddStudentInGroupAsync([FromRoute] int studentId, [FromRoute] int groupId)
    {
        await _universityGroupService.AddStudentInGroupAsync(studentId, groupId); 
        return Ok($"Successfully added student with id {studentId} in group with id {groupId}");
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPatch("{groupId:int}/add-students")]
    public async Task<ActionResult> AddMultipleStudentsInGroupAsync([FromRoute] int groupId, [FromBody] List<int> studentIds)
    {
        await _universityGroupService.AddMultipleStudentsInGroupAsync(groupId, studentIds); 
        return Ok("Successfull request!");
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UniversityGroupDto>> GetUniversityGroupByIdAsync([FromRoute] int id)
    {
        var existingGroup = await _universityGroupService.GetUniversityGroupByIdAsync(id);
        return Ok(_mapper.Map<UniversityGroupDto>(existingGroup));
    }

    [WithClaimsAuthorization("Admin")]
    [HttpGet]
    public async Task<ActionResult<List<UniversityGroupDto>>> GetAllUniversityGroupsAsync()
    {
        return Ok((await _universityGroupService.GetAllUniversityGroupsAsync())
            .Select(group => _mapper.Map<UniversityGroupDto>(group))
            .ToList());
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<UniversityGroupDto>>> GetOrderedUniversityGroupsAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _universityGroupService.GetOrderedUniversityGroupsAsync(paginationSetting);
        return Ok(new DatabaseFeedback<UniversityGroupDto>
        {
            Entities = databaseFeedback.Entities.Select(group => _mapper.Map<UniversityGroupDto>(group)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<UniversityGroupDto>>> GetFilteredUniversityGroupsAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableGroups = await _universityGroupService.GetFilteredUniversityGroupsAsync(filteringSettings);
        return Ok(new DatabaseFeedback<UniversityGroupDto>
        {
            Entities = queryableGroups.Entities.Select(group => _mapper.Map<UniversityGroupDto>(group)).ToList(),
            NumberOfEntities = queryableGroups.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<UniversityGroupDto>>> GetFilteredAndOrderedUniversityGroupsAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _universityGroupService.GetFilteredAndOrderedUniversityGroupsAsync(settings);
        return Ok(new DatabaseFeedback<UniversityGroupDto>
        {
            Entities = databaseFeedback.Entities.Select(group => _mapper.Map<UniversityGroupDto>(group)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateUniversityGroupByIdAsync([FromRoute] int id, [FromBody] UniversityUpdatedInputDto group)
    {
        var universityGroupJson = HttpContext.Items["RequestBody"] as string;
        await _universityGroupService.UpdateUniversityGroupByIdAsync(id, _mapper.Map<UniversityGroup>(group), universityGroupJson!);
        return Ok($"Successfully updated a group having the id {id}");
    }


    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUniversityGroupByIdAsync([FromRoute] int id)
    {
        await _universityGroupService.DeleteUniversityGroupByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllUniversityGroupsAsync()
    {
        await _universityGroupService.DeleteAllUniversityGroupsAsync();
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{groupId:int}/student/{studentId:int}")]
    public async Task<ActionResult> RemoveStudentFromGroupAsync([FromRoute] int studentId, [FromRoute] int groupId)
    {
        await _universityGroupService.RemoveStudentFromGroupAsync(studentId, groupId);
        return Ok($"Successfully removed student with id {studentId} from group with id {groupId}");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{groupId:int}/students")]
    public async Task<ActionResult> RemoveAllStudentsFromGroupAsync([FromRoute] int groupId)
    {
        await _universityGroupService.RemoveAllStudentsFromGroupAsync(groupId);
        return Ok($"Successfully removed students from group with id {groupId}");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost("{groupId:int}/students")]
    public async Task<ActionResult<DatabaseFeedback<StudentDto>>> GetAllStudentsFromGroupAsync([FromRoute] int groupId, [FromBody] FilterOrderSettings settings)
    {
        var queryableGroups = await _universityGroupService.GetOrderedAndFilteredStudentsFromGroupAsync(settings, groupId);
        return Ok(new DatabaseFeedback<StudentDto>
        {
            Entities = queryableGroups.Entities.Select(student => _mapper.Map<StudentDto>(student)).ToList(),
            NumberOfEntities = queryableGroups.NumberOfEntities
        });
    }
}