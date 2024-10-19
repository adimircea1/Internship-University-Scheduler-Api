using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using OnEntitySharedLogic.Models;
using StudentExamination.Api.Core.Models;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Models.ModelDtos.ExamDtos;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Presentation.Controllers;

[ApiController]
[Route("exams")]
public class ExamController : ControllerBase
{
    private readonly IExamService _examService;
    private readonly IMapper _mapper;

    public ExamController(
        IExamService examService,
        IMapper mapper)
    {
        _examService = examService;
        _mapper = mapper;
    }

    [ApiExplorerSettings(IgnoreApi = false)]
    [Authorize]
    [HttpPost("grade-exam/{examId:int}/{studentId:int}")]
    public async Task<ActionResult> GradeExamAsync([FromRoute] int examId, [FromRoute] int studentId, [FromBody] List<ProvidedAnswers> answers)
    {
        await _examService.GradeExamAsync(examId, studentId, answers);
        return Ok($"Successfully graded exam!");
    }

    [Authorize]
    [HttpGet("generate/{examId:int}/{studentId:int}")]
    public async Task<ActionResult<object>> GenerateExamAsync([FromRoute] int examId, [FromRoute] int studentId)
    {
        var examContentToSend = await _examService.GenerateExamAsync(examId, studentId);
        return examContentToSend;
    }
    
    [Authorize]
    [HttpGet("available/{studentId:int}")]
    public async Task<ActionResult> GetAvailableExamsOfStudent(int studentId)
    {
        return Ok((await _examService.GetAvailableExamsOfStudent(studentId))
            .Select(exam => _mapper.Map<ExamDto>(exam))
            .ToList());
    }

    [Authorize]
    [HttpGet("unavailable/{studentId:int}")]
    public async Task<ActionResult> GetUnavailableExamsOfStudent(int studentId)
    {
        return Ok((await _examService.GetUnavailableExamsOfStudent(studentId))
            .Select(exam => _mapper.Map<ExamDto>(exam))
            .ToList());
    }

    [Authorize]
    [HttpGet("all/{studentId:int}")]
    public async Task<ActionResult> GetAllExamsOfStudent(int studentId)
    {
        return Ok((await _examService.GetAllExamsOfStudent(studentId))
            .Select(exam => _mapper.Map<ExamDto>(exam))
            .ToList());
    }
    
    [Authorize]
    [HttpGet("{examId:int}/remaining-time")]
    public async Task<ActionResult<ExamRemainingTime>> GetExamRemainingTimeAsync([FromRoute] int examId)
    {
        return Ok(await _examService.GetExamRemainingTimeAsync(examId));
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPost("exam")]
    public async Task<ActionResult> AddExamAsync([FromBody] ExamInputDto exam)
    {
        await _examService.AddExamAsync(_mapper.Map<Exam>(exam));
        return Ok("Successfully added an examUpdatedInput!");
    }

    [WithClaimsAuthorization("Professor", "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddExamsAsync([FromBody] List<ExamInputDto> exams)
    {
        await _examService.AddExamsAsync(exams
            .Select(exam => _mapper.Map<Exam>(exam))
            .ToList());
        return Ok("Successfully added a list of exams!");
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExamDto>> GetExamById([FromRoute] int id)
    {
        var existingExam = await _examService.GetExamByIdAsync(id);
        return Ok(_mapper.Map<ExamDto>(existingExam));
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpGet]
    public async Task<ActionResult<List<ExamDto>>> GetAllExamsAsync()
    {
        return Ok((await _examService.GetAllExamsAsync())
            .Select(exam => _mapper.Map<ExamDto>(exam))
            .ToList());
    }

    [WithClaimsAuthorization("Student", "Professor")]
    [HttpPost("ordered")]
    public async Task<ActionResult<DatabaseFeedback<ExamDto>>> GetOrderedExamsAsync([FromBody] PaginationSetting paginationSetting)
    {
        var databaseFeedback = await _examService.GetOrderedExamsAsync(paginationSetting);
        return Ok(new DatabaseFeedback<ExamDto>
        {
            Entities = databaseFeedback.Entities.Select(exam => _mapper.Map<ExamDto>(exam)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }

    [WithClaimsAuthorization("Student", "Professor")]
    [HttpPost("filtered")]
    public async Task<ActionResult<DatabaseFeedback<ExamDto>>> GetFilteredExamsAsync([FromBody] FilteringSettings filteringSettings)
    {
        var queryableExams = await _examService.GetFilteredExamsAsync(filteringSettings);
        return Ok(new DatabaseFeedback<ExamDto>
        {
            Entities = queryableExams.Entities.Select(exam => _mapper.Map<ExamDto>(exam)).ToList(),
            NumberOfEntities = queryableExams.NumberOfEntities
        });
    }

    [WithClaimsAuthorization("Student", "Professor")]
    [HttpPost("filtered-ordered")]
    public async Task<ActionResult<DatabaseFeedback<ExamDto>>> GetFilteredAndOrderedExamsAsync([FromBody] FilterOrderSettings settings)
    {
        var databaseFeedback = await _examService.GetFilteredAndOrderedExamsAsync(settings);
        return Ok(new DatabaseFeedback<ExamDto>
        {
            Entities = databaseFeedback.Entities.Select(exam => _mapper.Map<ExamDto>(exam)).ToList(),
            NumberOfEntities = databaseFeedback.NumberOfEntities
        });
    }


    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateExamByIdAsync([FromRoute] int id, [FromBody] ExamUpdatedInputDto examUpdatedInput)
    {
        var examJson = HttpContext.Items["RequestBody"] as string;
        await _examService.UpdateExamByIdAsync(id, _mapper.Map<Exam>(examUpdatedInput), examJson!);
        return Ok($"Successfully updated exam with id {id}");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteExamById([FromRoute] int id)
    {
        await _examService.DeleteExamByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllExams()
    {
        await _examService.DeleteAllExamsAsync();
        return NoContent();
    }
}