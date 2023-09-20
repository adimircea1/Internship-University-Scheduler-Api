using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Internship.UniversityScheduler.Library.SharedEnums;
using OnEntitySharedLogic.Utils;

namespace Internship.UniversityScheduler.Api.Core.Models;

public class Professor : IEntity
{
    [Column(TypeName = "varchar(50)")] 
    public string FirstName { get; set; } = string.Empty;
    
    [Column(TypeName = "varchar(50)")] 
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthdayDate { get; set; }
    
    [EmailAddress] 
    public string Email { get; set; } = string.Empty;

    [Column(TypeName = "varchar(30)")]
    public string PhoneNumber { get; set; } = string.Empty;

    public ProfessorSpeciality Speciality { get; set; }
    public List<Course> Courses { get; set; } = new();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}