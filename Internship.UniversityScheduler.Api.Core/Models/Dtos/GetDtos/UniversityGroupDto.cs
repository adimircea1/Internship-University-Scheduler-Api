using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;

public class UniversityGroupDto
{
    public int Id { get; set; }
    public int NumberOfMembers { get; set; }
    public int MaxSize { get; set; }
    public string Name { get; set; } = string.Empty;
    public UniversitySpecialization Specialization { get; set; }
}