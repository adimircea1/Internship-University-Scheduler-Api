using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Models.ModelDtos.ProblemDtos;

public class ProblemDto
{
    public int Id { get; set; }
    public double Points { get; set; }
    public string Text { get; set; } = string.Empty;
    public int ExamId { get; set; }
    public ProblemType ProblemType { get; set; }
}