using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;

public class CourseDto
{
    public int Id { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int NumberOfCredits { get; set; }
    public CourseType Type { get; set; }
    public CourseDomain Domain { get; set; }
    public int ProfessorId { get; set; }
}