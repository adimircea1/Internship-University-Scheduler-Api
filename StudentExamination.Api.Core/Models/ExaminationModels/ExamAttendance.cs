using OnEntitySharedLogic.Utils;

namespace StudentExamination.Api.Core.Models.ExaminationModels;

public class ExamAttendance : IEntity
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public Exam? Exam { get; set; }
}