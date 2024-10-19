using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PatchDto;

public class CourseUpdatedInputDto
{
    public string CourseName { get; set; } = string.Empty;
    public int NumberOfCredits { get; set; }
    public CourseType Type { get; set; }
    public CourseDomain Domain { get; set; }
}