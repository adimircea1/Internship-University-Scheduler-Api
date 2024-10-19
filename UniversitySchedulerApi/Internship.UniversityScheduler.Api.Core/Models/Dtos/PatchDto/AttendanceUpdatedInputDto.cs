namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;

public class AttendanceUpdatedInputDto
{
    public DateOnly DateOfTheCourse { get; set; }
    public TimeOnly TimeOfTheCourse { get; set; }
}