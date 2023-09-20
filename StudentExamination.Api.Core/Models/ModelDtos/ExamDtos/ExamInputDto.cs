namespace StudentExamination.Api.Core.Models.ModelDtos.ExamDtos;

public class ExamInputDto
{
    public int CourseId { get; set; }
    public int ExamDuration { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableUntil { get; set; }
    public bool PartialGradingAllowed { get; set; } = false;
}