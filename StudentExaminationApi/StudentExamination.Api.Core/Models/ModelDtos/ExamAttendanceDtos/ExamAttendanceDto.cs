namespace StudentExamination.Api.Core.Models.ModelDtos.ExamAttendanceDtos;

public class ExamAttendanceDto
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
}