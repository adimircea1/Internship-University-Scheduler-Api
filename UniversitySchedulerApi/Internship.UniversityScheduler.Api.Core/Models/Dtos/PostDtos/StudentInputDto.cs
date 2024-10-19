namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;

public class StudentInputDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PersonalEmail { get; set; } = string.Empty;
    public DateOnly BirthdayDate { get; set; }
    public int StudyYear { get; set; }
}