using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Models.ModelDtos.ExamAttendanceDtos;
using StudentExamination.Api.Core.Models.ModelDtos.ExamDtos;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Presentation.Controllers;

[ApiController]
[Route("exam-attendances")]
public class ExamAttendanceController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IExamAttendanceService _examAttendanceService;

    public ExamAttendanceController(
        IMapper mapper,
        IExamAttendanceService examAttendanceService)
    {
        _mapper = mapper;
        _examAttendanceService = examAttendanceService;
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost("attendance")]
    public async Task<ActionResult> AddExamAttendanceAsync([FromBody] ExamAttendanceInputDto attendance)
    {
        await _examAttendanceService.AddExamAttendanceAsync(_mapper.Map<ExamAttendance>(attendance));
        return Ok("Successfully added an exam attendance!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost]
    public async Task<ActionResult> AddExamAttendancesAsync([FromBody] List<ExamAttendanceInputDto> attendances)
    {
        await _examAttendanceService.AddMultipleExamAttendancesAsync(attendances.Select(attendance => _mapper.Map<ExamAttendance>(attendance))
            .ToList());
        return Ok("Successfully added a list of exam attendances!");
    }
    
    [WithClaimsAuthorization("Student")]
    [HttpGet("/student/{studentId:int}")]
    public async Task<ActionResult> GetExamAttendancesOfStudentAsync([FromRoute] int studentId)
    {
        var attendances = await _examAttendanceService.GetExamAttendancesOfStudentAsync(studentId);
        return Ok(attendances.Select(attendance => _mapper.Map<ExamDto>(attendance)).ToList());
    }

    [WithClaimsAuthorization("Admin", "Professor", "Student")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExamAttendanceDto>> GetExamAttendanceById([FromRoute] int id)
    {
        var existingExamAttendance = await _examAttendanceService.GetExamAttendanceByQueryAsync(attendance => attendance.Id == id);
        return Ok(_mapper.Map<ExamAttendanceDto>(existingExamAttendance));
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteExamAttendanceById([FromRoute] int id)
    {
        await _examAttendanceService.DeleteExamAttendanceByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllExamAttendances()
    {
        await _examAttendanceService.DeleteAllExamAttendancesAsync();
        return NoContent();
    }
}
