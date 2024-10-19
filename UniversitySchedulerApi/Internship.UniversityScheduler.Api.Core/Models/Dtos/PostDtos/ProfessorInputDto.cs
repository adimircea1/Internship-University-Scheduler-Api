using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;

public class ProfessorInputDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly BirthdayDate { get; set; }
    public ProfessorSpeciality Speciality { get; set; }
}