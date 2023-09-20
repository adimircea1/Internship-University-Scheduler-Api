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
[Route("courses")]
public class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly IMapper _mapper;

    public CourseController(
        ICourseService courseService,
        IMapper mapper)
    {
        _courseService = courseService;
        _mapper = mapper;
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost("course")]
    public async Task<ActionResult> AddCourseAsync([FromBody] CourseInputDto course)
    {
        await _courseService.AddCourseAsync(_mapper.Map<Course>(course));
        return Ok("Successfully added a course!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost]
    public async Task<ActionResult> AddCoursesAsync([FromBody] List<CourseInputDto> courses)
    {
        await _courseService.AddCoursesAsync(courses
            .Select(course => _mapper.Map<Course>(course))
            .ToList());
        return Ok("Successfully added a list of courses!");
    }

    [WithClaimsAuthorization("Professor", "Student")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CourseDto>> GetCourseByIdAsync([FromRoute] int id)
    {
        var existingCourse = await _courseService.GetCourseByIdAsync(id);
        return Ok(_mapper.Map<CourseDto>(existingCourse));
    }

    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpGet]
    public async Task<ActionResult<List<CourseDto>>> GetAllCoursesAsync()
    {
        return Ok((await _courseService.GetAllCoursesAsync())
            .Select(course => _mapper.Map<CourseDto>(course))
            .ToList());
    }

    [WithClaimsAuthorization("Admin")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<CourseDto>>> GetOrderedCoursesAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _courseService.GetOrderedCoursesAsync(paginationSetting);
        return Ok(new DatabaseFeedback<CourseDto>
        {
            Entities = databaseFeedback.Entities.Select(course => _mapper.Map<CourseDto>(course)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<CourseDto>>> GetFilteredCoursesAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableCourses = await _courseService.GetFilteredCoursesAsync(filteringSettings);
        return Ok(new DatabaseFeedback<CourseDto>
        {
            Entities = queryableCourses.Entities.Select(course => _mapper.Map<CourseDto>(course)).ToList(),
            NumberOfEntities = queryableCourses.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<CourseDto>>> GetFilteredAndOrderedCoursesAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _courseService.GetFilteredAndOrderedCoursesAsync(settings);
        return Ok(new DatabaseFeedback<CourseDto>
        {
            Entities = databaseFeedback.Entities.Select(course => _mapper.Map<CourseDto>(course)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Admin")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateCourseByIdAsync([FromRoute] int id, [FromBody] CourseUpdatedInputDto course)
    {
        var courseJson = HttpContext.Items["RequestBody"] as string;
        await _courseService.UpdateCourseByIdAsync(id, _mapper.Map<Course>(course), courseJson!);
        return Ok($"Successfully updated course with id {id}!");
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCourseById([FromRoute] int id)
    {
        await _courseService.DeleteCourseByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllCoursesAsync()
    {
        await _courseService.DeleteAllCoursesAsync();
        return NoContent();
    }
}