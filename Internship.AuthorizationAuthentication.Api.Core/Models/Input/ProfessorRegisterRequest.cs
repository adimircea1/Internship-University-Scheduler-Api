using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Internship.UniversityScheduler.Library.SharedEnums;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.Input;

public class ProfessorRegisterRequest
{
    [Required] 
    [EmailAddress] 
    public string Email { get; set; } = string.Empty;

    [Required] 
    public string FirstName { get; set; } = string.Empty;

    [Required] 
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "varchar(30)")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    public DateOnly Birthdate { get; set; }

    [Required] 
    public ProfessorSpeciality Speciality { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}