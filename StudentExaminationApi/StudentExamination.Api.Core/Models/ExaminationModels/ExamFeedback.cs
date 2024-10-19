namespace StudentExamination.Api.Core.Models.ExaminationModels;

public class ExamFeedback
{
    public int FinalGrade { get; set; }
    public Dictionary<int, List<string>> CorrectAnswersOfProblems { get; set; } = new();
}