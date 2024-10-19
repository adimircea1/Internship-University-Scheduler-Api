using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;

public class ProfessorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? BirthdayDate { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public ProfessorSpeciality Speciality { get; set; }
}