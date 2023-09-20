namespace Internship.UniversityScheduler.Library.SharedModels;

public class GradeEmail
{
    public int GradeValue { get; set; }
    public string CourseDomain { get; set; } = string.Empty;
    public DateTime DateOfGrade { get; set; }
}