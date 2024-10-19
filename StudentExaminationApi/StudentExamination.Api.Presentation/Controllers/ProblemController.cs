using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Models.ModelDtos.ProblemAnswerDtos;
using StudentExamination.Api.Core.Models.ModelDtos.ProblemDtos;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Presentation.Controllers;

[ApiController]
[Route("problems")]
public class ProblemController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProblemService _problemService;

    public ProblemController(
        IProblemService problemService,
        IMapper mapper)
    {
        _problemService = problemService;
        _mapper = mapper;
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost("problem")]
    public async Task<ActionResult> AddProblemAsync([FromBody] ProblemInputDto problem)
    {
        await _problemService.AddProblemAsync(_mapper.Map<Problem>(problem));
        return Ok("Successfully added a problem!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost]
    public async Task<ActionResult> AddProblemsAsync([FromBody] List<ProblemInputDto> problems)
    {
        await _problemService.AddProblemsAsync(problems
            .Select(problem => _mapper.Map<Problem>(problem))
            .ToList());
        return Ok("Successfully added a list of problems!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProblemDto>> GetProblemById([FromRoute] int id)
    {
        var existingProblem = await _problemService.GetProblemByIdAsync(id);
        return Ok(_mapper.Map<ProblemDto>(existingProblem));
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpGet]
    public async Task<ActionResult<List<ProblemDto>>> GetAllProblemsAsync()
    {
        return Ok((await _problemService.GetAllProblemsAsync())
            .Select(problem => _mapper.Map<ProblemDto>(problem))
            .ToList());
    }

    [Authorize]
    [HttpGet("answer-options/{problemId:int}")]
    public async Task<ActionResult<List<AnswerOptionDto>>> GetAllAnswerOptionsOfProblemAsync([FromRoute] int problemId)
    {
        return Ok((await _problemService.GetAllAnswerOptionsForProblemWithId(problemId))
            .Select(problem => _mapper.Map<AnswerOptionDto>(problem))
            .ToList());
    }
    
    [Authorize]
    [HttpGet("correct-answers/{problemId:int}")]
    public async Task<ActionResult<List<CorrectAnswerDto>>> GetAllCorrectAnswersOfProblemAsync([FromRoute] int problemId)
    {
        return Ok((await _problemService.GetAllCorrectAnswersForProblemWithId(problemId))
            .Select(problem => _mapper.Map<CorrectAnswerDto>(problem))
            .ToList());
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> UpdateProblemByIdAsync([FromRoute] int id, [FromBody] ProblemUpdatedInputDto problemUpdatedInput)
    {
        var problemJson = HttpContext.Items["RequestBody"] as string;
        await _problemService.UpdateProblemByIdAsync(id, _mapper.Map<Problem>(problemUpdatedInput), problemJson!);
        return Ok($"Successfully updated problem with id {id}!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProblemById([FromRoute] int id)
    {
        await _problemService.DeleteProblemByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllProblems()
    {
        await _problemService.DeleteAllProblemsAsync();
        return NoContent();
    }
}