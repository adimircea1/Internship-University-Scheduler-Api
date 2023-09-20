using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;

public class ProfessorUpdatedInputDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly BirthdayDate { get; set; }
    public ProfessorSpeciality Speciality { get; set; }
}