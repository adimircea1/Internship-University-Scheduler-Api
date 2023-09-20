namespace Internship.UniversityScheduler.Api.Core.Models.Dtos.GetDtos;

public class GradeDto
{
    public int Id { get; set; }
    public int Value { get; set; }
    public int CatalogueId { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime DateOfGrade { get; set; }
}