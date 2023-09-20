namespace StudentExamination.Api.Core.Models.ModelDtos.ProblemAnswerDtos;

public class CorrectAnswerDto
{
    public int Id { get; set; }
    public int ProblemId { get; set; }
    public string Answer { get; set; } = string.Empty;
}