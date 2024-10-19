namespace StudentExamination.Api.Core.Models.ModelDtos.ExamDtos;

public class ExamUpdatedInputDto
{
    public DateTime? AvailableFrom { get; set; }
    public DateTime? AvailableUntil { get; set; }
    public int? FinalGrade { get; set; }
    public bool? PartialGradingAllowed { get; set; }
}