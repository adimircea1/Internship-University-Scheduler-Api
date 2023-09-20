namespace StudentExamination.Api.Core.Models.ModelDtos.ProblemAnswerDtos;

public class AnswerOptionInputDto
{
    public int ProblemId { get; set; }
    public string Answer { get; set; } = string.Empty;
}