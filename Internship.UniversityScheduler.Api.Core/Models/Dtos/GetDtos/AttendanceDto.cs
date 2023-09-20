namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;

public class AttendanceDto
{
    public int Id { get; set; }
    public DateOnly DateOfTheCourse { get; set; }
    public TimeOnly TimeOfTheCourse { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
}