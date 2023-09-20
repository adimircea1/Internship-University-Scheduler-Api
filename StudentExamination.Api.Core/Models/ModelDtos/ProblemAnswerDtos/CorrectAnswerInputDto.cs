namespace StudentExamination.Api.Core.Models.ModelDtos.ProblemAnswerDtos;

public class CorrectAnswerInputDto
{
    public int ProblemId { get; set; }
    public string Answer { get; set; } = string.Empty;
}