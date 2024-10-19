using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models;

public class Student : IEntity
{
    [Validate(0, 4)] 
    [DefaultValue(1)]
    public int StudyYear { get; set; }
    
    [Column(TypeName = "varchar(50)")] 
    public string FirstName { get; set; } = string.Empty;

    [Column(TypeName = "varchar(50)")]
    public string LastName { get; set; } = string.Empty;

    [Column(TypeName = "varchar(100)")]
    public string FullName { get; set; } = string.Empty;
    
    public DateOnly BirthdayDate { get; set; }

    [EmailAddress] 
    public string Email { get; set; } = string.Empty;

    [EmailAddress] 
    public string PersonalEmail { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar(30)")]
    public string PhoneNumber { get; set; } = string.Empty;

    public int? UniversityGroupId { get; set; }
    public UniversityGroup? UniversityGroup { get; set; }
    public List<Attendance> Attendances { get; set; } = new();
    public List<Grade> Grades { get; set; } = new();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}