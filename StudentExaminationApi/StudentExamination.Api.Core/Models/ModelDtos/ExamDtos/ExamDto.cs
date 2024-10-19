namespace StudentExamination.Api.Core.Models.ModelDtos.ExamDtos;

public class ExamDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableUntil { get; set; }
    public int ExamDuration { get; set; }
    public int? FinalGrade { get; set; }
}