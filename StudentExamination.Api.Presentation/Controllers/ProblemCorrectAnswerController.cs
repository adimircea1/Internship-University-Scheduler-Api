using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Models.ModelDtos.ProblemAnswerDtos;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Presentation.Controllers;

[ApiController]
[Route("correct-answers")]
public class ProblemCorrectAnswerController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ICorrectAnswerService _correctAnswerService;

    public ProblemCorrectAnswerController(
        IMapper mapper,
        ICorrectAnswerService correctAnswerService)
    {
        _mapper = mapper;
        _correctAnswerService = correctAnswerService;
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost("answer")]
    public async Task<ActionResult> AddProblemCorrectAnswerAsync([FromBody] CorrectAnswerInputDto problem)
    {
        await _correctAnswerService.AddCorrectAnswerAsync(_mapper.Map<CorrectAnswer>(problem));
        return Ok("Successfully added a problem correct answer!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost]
    public async Task<ActionResult> AddProblemCorrectAnswersAsync([FromBody] List<CorrectAnswerInputDto> problems)
    {
        await _correctAnswerService.AddMultipleCorrectAnswersAsync(problems
            .Select(problem => _mapper.Map<CorrectAnswer>(problem))
            .ToList());
        return Ok("Successfully added a list of problem correct answers!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CorrectAnswerDto>> GetProblemCorrectAnswerById([FromRoute] int id)
    {
        var existingProblemCorrectAnswer = await _correctAnswerService.GetCorrectAnswerByQueryAsync(answer => answer.Id == id);
        return Ok(_mapper.Map<CorrectAnswerDto>(existingProblemCorrectAnswer));
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProblemCorrectAnswerById([FromRoute] int id)
    {
        await _correctAnswerService.DeleteCorrectAnswerByIdAsync(id);
        return NoContent();
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<AnswerOptionDto>> UpdateAnswerOptionByIdAsync([FromRoute] int id, [FromBody] CorrectAnswerUpdatedInputDto correctAnswerUpdatedInput)
    {
        var correctAnswerJson = HttpContext.Items["RequestBody"] as string;
        await _correctAnswerService.UpdateCorrectAnswerByIdAsync(id, _mapper.Map<CorrectAnswer>(correctAnswerUpdatedInput), correctAnswerJson!);
        return Ok($"Successfully updated correct answer with id {id}!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllProblemCorrectAnswers()
    {
        await _correctAnswerService.DeleteAllCorrectAnswersAsync();
        return NoContent();
    }
}
