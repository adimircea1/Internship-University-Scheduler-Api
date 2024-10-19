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
[Route("grades")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;
    private readonly IMapper _mapper;

    public GradeController(
        IGradeService gradeService,
        IMapper mapper)
    {
        _gradeService = gradeService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost("grade")]
    public async Task<ActionResult> AddGradeAsync([FromBody] GradeInputDto grade)
    {
        await _gradeService.AddGradeAsync(_mapper.Map<Grade>(grade));
        return Ok("Successfully added a grade!");
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddGradesAsync([FromBody] List<GradeInputDto> grades)
    {
        await _gradeService.AddGradesAsync(grades
            .Select(grade => _mapper.Map<Grade>(grade))
            .ToList());
        return Ok("Successfully added a list of grades!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GradeDto>> GetGradeByIdAsync([FromRoute] int id)
    {
        var existingGrade = await _gradeService.GetGradeByIdAsync(id);
        return Ok(_mapper.Map<GradeDto>(existingGrade));
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpGet]
    public async Task<ActionResult<List<GradeDto>>> GetAllGradesAsync()
    {
        return Ok((await _gradeService.GetAllGradesAsync())
            .Select(grade => _mapper.Map<GradeDto>(grade))
            .ToList());
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpGet("catalogue/{catalogueId:int}")]
    public async Task<ActionResult<List<GradeDto>>> GetAllGradesFromCatalogueAsync([FromRoute] int catalogueId)
    {
        return Ok((await _gradeService.GetAllGradesFromCatalogueAsync(catalogueId))
            .Select(grade => _mapper.Map<GradeDto>(grade))
            .ToList());
    }

    [WithClaimsAuthorization("Professor", "Student")]
    [HttpGet("student/{studentId:int}")]
    public async Task<ActionResult<List<GradeDto>>> GetAllGradesOfAStudentAsync([FromRoute] int studentId)
    {
        return Ok((await _gradeService.GetAllGradesOfAStudentAsync(studentId))
            .Select(grade => _mapper.Map<GradeDto>(grade))
            .ToList());
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpGet("course/{courseId:int}")]
    public async Task<ActionResult<List<GradeDto>>> GetAllGradesFromCourseAsync([FromRoute] int courseId)
    {
        return Ok((await _gradeService.GetAllGradesFromCourseAsync(courseId))
            .Select(grade => _mapper.Map<GradeDto>(grade))
            .ToList());
    }
    
    [WithClaimsAuthorization("Professor", "Student")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<GradeDto>>> GetOrderedGradesAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _gradeService.GetOrderedGradesAsync(paginationSetting);
        return Ok(new DatabaseFeedback<GradeDto>
        {
            Entities = databaseFeedback.Entities.Select(grade => _mapper.Map<GradeDto>(grade)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }

    [WithClaimsAuthorization("Professor", "Student")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<GradeDto>>> GetFilteredGradesAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableGrades = await _gradeService.GetFilteredGradesAsync(filteringSettings);
        return Ok(new DatabaseFeedback<GradeDto>
        {
            Entities = queryableGrades.Entities.Select(grade => _mapper.Map<GradeDto>(grade)).ToList(),
            NumberOfEntities = queryableGrades.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Professor", "Student")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<GradeDto>>> GetFilteredAndOrderedGradesAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _gradeService.GetFilteredAndOrderedGradesAsync(settings);
        return Ok(new DatabaseFeedback<GradeDto>
        {
            Entities = databaseFeedback.Entities.Select(grade => _mapper.Map<GradeDto>(grade)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateGradeByIdAsync([FromRoute] int id, [FromBody] GradeUpdatedInputDto grade)
    {
        var gradeJson = HttpContext.Items["RequestBody"] as string;
        await _gradeService.UpdateGradeByIdAsync(id, _mapper.Map<Grade>(grade), gradeJson!);
        return Ok($"Successfully updated grade with id {id}!");
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGradeById([FromRoute] int id)
    {
        await _gradeService.DeleteGradeByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllGradesAsync()
    {
        await _gradeService.DeleteAllGradesAsync();
        return NoContent();
    }
}