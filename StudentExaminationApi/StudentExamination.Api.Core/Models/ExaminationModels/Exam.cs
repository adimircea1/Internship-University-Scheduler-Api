using System.ComponentModel.DataAnnotations.Schema;
using OnEntitySharedLogic.Utils;

namespace StudentExamination.Api.Core.Models.ExaminationModels;

public class Exam : IEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int? FinalGrade { get; set; }
    public int ExamDuration;
    public DateTime AvailableFrom { get; set; }
    public DateTime AvailableUntil { get; set; }
    public DateTime? FirstVisitTime { get; set; }
    public bool PartialGradingAllowed { get; set; }
    public List<Problem> Problems { get; set; } = new();
    public List<ExamAttendance> ExamAttendances { get; set; } = new();
}