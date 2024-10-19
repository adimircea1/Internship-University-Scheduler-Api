namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;

public class StudentDto
{
    public int Id { get; set; }
    public int StudyYear { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly? BirthdayDate { get; set; }
    public string PersonalEmail { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int? UniversityGroupId { get; set; }
}