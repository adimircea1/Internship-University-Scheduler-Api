using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OnEntitySharedLogic.CustomAuthorizationAttributeWithClaims;
using StudentExamination.Api.Core.Models.ExaminationModels;
using StudentExamination.Api.Core.Models.ModelDtos.ProblemAnswerDtos;
using StudentExamination.Api.Core.Services.Interfaces;

namespace StudentExamination.Api.Presentation.Controllers;

[ApiController]
[Route("answer-options")]
public class ProblemAnswerOptionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAnswerOptionService _answerOptionService;

    public ProblemAnswerOptionController(
        IMapper mapper, 
        IAnswerOptionService answerOptionService)
    {
        _mapper = mapper;
        _answerOptionService = answerOptionService;
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost("answer")]
    public async Task<ActionResult> AddAnswerOptionAsync([FromBody] AnswerOptionInputDto problem)
    {
        await _answerOptionService.AddAnswerOptionAsync(_mapper.Map<AnswerOption>(problem));
        return Ok("Successfully added a answer option!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPost]
    public async Task<ActionResult> AddAnswerOptionsAsync([FromBody] List<AnswerOptionInputDto> problems)
    {
        await _answerOptionService.AddMultipleAnswerOptionsAsync(problems
            .Select(problem => _mapper.Map<AnswerOption>(problem))
            .ToList());
        return Ok("Successfully added a list of answer options!");
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AnswerOptionDto>> GetAnswerOptionById([FromRoute] int id)
    {
        var existingProblemAnswerOption = await _answerOptionService.GetAnswerOptionByQueryAsync(answer => answer.Id == id);
        return Ok(_mapper.Map<AnswerOptionDto>(existingProblemAnswerOption));
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<AnswerOptionDto>> UpdateAnswerOptionByIdAsync([FromRoute] int id, [FromBody] AnswerOptionUpdatedInputDto answerOptionUpdatedInput)
    {
        var answerOptionJson = HttpContext.Items["RequestBody"] as string;
        await _answerOptionService.UpdateAnswerOptionByIdAsync(id, _mapper.Map<AnswerOption>(answerOptionUpdatedInput), answerOptionJson!);
        return Ok($"Successfully updated answer option with id {id}!");
    }
    
    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAnswerOptionById([FromRoute] int id)
    {
        await _answerOptionService.DeleteAnswerOptionByIdAsync(id);
        return NoContent();
    }

    [WithClaimsAuthorization("Admin", "Professor")]
    [HttpDelete]
    public async Task<ActionResult> DeleteAllAnswerOptions()
    {
        await _answerOptionService.DeleteAllAnswerOptionsAsync();
        return NoContent();
    }
}