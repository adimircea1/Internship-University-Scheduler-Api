namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.PostDtos;

public class GradeInputDto
{
    public int CatalogueId { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public int Value { get; set; }
    public DateTime DateOfGrade { get; set; }
}