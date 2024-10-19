using OnEntitySharedLogic.Utils;

namespace StudentExamination.Api.Core.Models.ExaminationModels;

public class Problem : IEntity
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    [Validate(0)]
    public int Points { get; set; }
    public string Text { get; set; } = string.Empty;
    public ProblemType ProblemType { get; set; }
    public List<AnswerOption> AnswerOptions = new();
    public List<CorrectAnswer> CorrectAnswers = new();
    public Exam? Exam { get; set; }
}