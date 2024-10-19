namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;

public class AttendanceInputDto
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateOnly DateOfTheCourse { get; set; }
    public TimeOnly TimeOfTheCourse { get; set; }
}