namespace Internship.UniversityScheduler.Library.SharedModels;

public class StudentGradesEmail
{
    public string StudentEmail { get; set; } = string.Empty;
    public List<GradeEmail> Grades { get; set; } = new();
}