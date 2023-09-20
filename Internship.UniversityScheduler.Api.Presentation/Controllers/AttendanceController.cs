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
[Route("attendances")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly IMapper _mapper;

    public AttendanceController(
        IAttendanceService attendanceService,
        IMapper mapper)
    {
        _attendanceService = attendanceService;
        _mapper = mapper;
    }

    [WithClaimsAuthorization("Professor")]
    [HttpPost("attendance")]
    public async Task<ActionResult> AddAttendanceAsync([FromBody] AttendanceInputDto attendance)
    {
        await _attendanceService.AddAttendanceAsync(_mapper.Map<Attendance>(attendance));
        return Ok("Successfully added an attendance!");
    }

    [WithClaimsAuthorization("Professor")]
    [HttpPost]
    public async Task<ActionResult> AddMultipleAttendanceAsync([FromBody] List<AttendanceInputDto> attendances)
    {
        await _attendanceService.AddAttendancesAsync(attendances
            .Select(attendance => _mapper.Map<Attendance>(attendance))
            .ToList());
        return Ok("Successfully added multiple attendances!");
    }

    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AttendanceDto>> GetAttendanceByIdAsync([FromRoute] int id)
    {
        var existingAttendance = await _attendanceService.GetAttendanceByIdAsync(id);
        return Ok(_mapper.Map<AttendanceDto>(existingAttendance));
    }

    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpGet]
    public async Task<ActionResult<List<AttendanceDto>>> GetAllAttendanceAsync()
    {
        return Ok((await _attendanceService.GetAllAttendancesAsync())
            .Select(attendance => _mapper.Map<AttendanceDto>(attendance))
            .ToList());
    }
    
    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<AttendanceDto>>> GetOrderedAttendancesAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _attendanceService.GetOrderedAttendancesAsync(paginationSetting);
        return Ok(new DatabaseFeedback<AttendanceDto>
        {
            Entities = databaseFeedback.Entities.Select(attendance => _mapper.Map<AttendanceDto>(attendance)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
     
    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<AttendanceDto>>> GetFilteredAttendancesAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableAttendances = await _attendanceService.GetFilteredAttendancesAsync(filteringSettings);
        return Ok(new DatabaseFeedback<AttendanceDto>
        {
            Entities = queryableAttendances.Entities.Select(attendance => _mapper.Map<AttendanceDto>(attendance)).ToList(),
            NumberOfEntities = queryableAttendances.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<AttendanceDto>>> GetFilteredAndOrderedAttendancesAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _attendanceService.GetFilteredAndOrderedAttendancesAsync(settings);
        return Ok(new DatabaseFeedback<AttendanceDto>
        {
            Entities = databaseFeedback.Entities.Select(attendance => _mapper.Map<AttendanceDto>(attendance)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }
    
    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpGet("course/{courseId:int}")]
    public async Task<ActionResult<List<AttendanceDto>>> GetCourseAttendancesAsync([FromRoute] int courseId)
    {
        return (await _attendanceService.GetCourseAttendancesAsync(courseId))
            .Select(attendance => _mapper.Map<AttendanceDto>(attendance))
            .ToList();
    }

    [WithClaimsAuthorization("Professor", "Admin", "Student")]
    [HttpGet("student/{studentId:int}")]
    public async Task<ActionResult<List<AttendanceDto>>> GetAttendancesOfStudentAsync([FromRoute] int studentId)
    {
        return (await _attendanceService.GetAttendancesOfStudentAsync(studentId))
            .Select(attendance => _mapper.Map<AttendanceDto>(attendance))
            .ToList();
    }

    [WithClaimsAuthorization("Professor")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateAttendanceByIdAsync([FromRoute] int id, [FromBody] AttendanceUpdatedInputDto attendance)
    {
        var attendanceJson = HttpContext.Items["RequestBody"] as string;
        await _attendanceService.UpdateAttendanceByIdAsync(id, _mapper.Map<Attendance>(attendance), attendanceJson!);
        return Ok($"Successfully updated attendance with id {id}!");
    }

    [WithClaimsAuthorization("Professor")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAttendanceById([FromRoute] int id)
    {
        await _attendanceService.DeleteAttendanceByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllAttendanceAsync()
    {
        await _attendanceService.DeleteAllAttendancesAsync();
        return NoContent();
    }
}