namespace StudentExamination.Api.Core.Models.ExaminationModels;

public class ProvidedAnswers
{
    public int ProblemId { get; set; }
    public List<string> Answers { get; set; } = new();
}