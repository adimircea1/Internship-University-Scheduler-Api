namespace StudentExamination.Api.Core.Models.ModelDtos.ProblemDtos;

public class ProblemUpdatedInputDto
{
    public string Text { get; set; } = string.Empty;
    public double Points { get; set; }
}