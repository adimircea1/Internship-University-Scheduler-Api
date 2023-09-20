using System.ComponentModel.DataAnnotations.Schema;
using Internship.UniversityScheduler.Library.SharedEnums;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models;

public class Course : IEntity
{
    [Validate(0)] 
    public int NumberOfCredits { get; set; }

    [Column(TypeName = "varchar(256)")] 
    public string CourseName { get; set; } = string.Empty;

    public CourseType Type { get; set; }
    public CourseDomain Domain { get; set; }
    public int ProfessorId { get; set; }
    public Professor? Professor { get; set; }
    public List<Attendance> Attendances { get; set; } = new();
    public List<Grade> Grades { get; set; } = new();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}