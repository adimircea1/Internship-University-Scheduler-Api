using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;

public class UniversityUpdatedInputDto
{
    public string Name { get; set; } = string.Empty;
    public UniversitySpecialization Specialization { get; set; }
}