using StudentExamination.Api.Core.Models.ExaminationModels;

namespace StudentExamination.Api.Core.Models.ModelDtos.ProblemDtos;

public class ProblemInputDto
{
    public string Text { get; set; } = string.Empty;

    public double Points { get; set; }

    public int ExamId { get; set; }
    
    public ProblemType ProblemType { get; set; }

}