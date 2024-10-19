using AutoMapper;
using Internship.UniversityScheduler.Api.Core.Models;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;
using Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;
using Internship.UniversityScheduler.Api.Core.ServiceClasses.Abstractions;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;

namespace Internship.UniversityScheduler.Api.Presentation.Controllers;

[ApiController]
[Route("professors")]
public class ProfessorController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProfessorService _professorService;

    public ProfessorController(
        IProfessorService professorService,
        IMapper mapper)
    {
        _professorService = professorService;
        _mapper = mapper;
    }


    [WithClaimsAuthorization("Admin")]
    [HttpPost("professor")]
    public async Task<ActionResult> AddProfessorAsync([FromBody] ProfessorInputDto professor)
    {
        await _professorService.AddProfessorAsync(_mapper.Map<Professor>(professor));
        return Ok("Successfully added a professor!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost]
    public async Task<ActionResult> AddProfessorsAsync([FromBody] List<ProfessorInputDto> professors)
    {
        await _professorService.AddProfessorsAsync(professors
            .Select(professor => _mapper.Map<Professor>(professor))
            .ToList());
        return Ok("Successfully added a list of professors!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProfessorDto>> GetProfessorByIdAsync([FromRoute] int id)
    {
        var existingProfessor = await _professorService.GetProfessorByIdAsync(id);
        return Ok(_mapper.Map<ProfessorDto>(existingProfessor));
    }

    [WithClaimsAuthorization("Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ProfessorDto>>> GetAllProfessorsAsync()
    {
        return Ok((await _professorService.GetAllProfessorsAsync())
            .Select(professor => _mapper.Map<ProfessorDto>(professor))
            .ToList());
    }
    
    [WithClaimsAuthorization("Professor")]
    [HttpGet("professor")]
    public async Task<ActionResult<ProfessorDto>> GetProfessorByClaimIdAsync()
    {
        return Ok(_mapper.Map<ProfessorDto>(await _professorService.GetProfessorByIdClaimAsync()));
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<ProfessorDto>>> GetOrderedProfessorsAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _professorService.GetOrderedProfessorsAsync(paginationSetting);
        return Ok(new DatabaseFeedback<ProfessorDto>
        {
            Entities = databaseFeedback.Entities.Select(professor => _mapper.Map<ProfessorDto>(professor)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<ProfessorDto>>> GetFilteredProfessorsAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableProfessors = await _professorService.GetFilteredProfessorsAsync(filteringSettings);
        return Ok(new DatabaseFeedback<ProfessorDto>
        {
            Entities = queryableProfessors.Entities.Select(professor => _mapper.Map<ProfessorDto>(professor)).ToList(),
            NumberOfEntities = queryableProfessors.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<ProfessorDto>>> GetFilteredAndOrderedProfessorsAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _professorService.GetFilteredAndOrderedProfessorsAsync(settings);
        return Ok(new DatabaseFeedback<ProfessorDto>
        {
            Entities = databaseFeedback.Entities.Select(professor => _mapper.Map<ProfessorDto>(professor)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateProfessorByIdAsync([FromRoute] int id, [FromBody] ProfessorUpdatedInputDto professor)
    {
        var professorJson = HttpContext.Items["RequestBody"] as string;
        await _professorService.UpdateProfessorByIdAsync(id, _mapper.Map<Professor>(professor), professorJson!);
        return Ok($"Successfully updated professor with id {id}!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProfessorById([FromRoute] int id)
    {
        await _professorService.DeleteProfessorByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllProfessorsAsync()
    {
        await _professorService.DeleteAllProfessorsAsync();
        return NoContent();
    }
}