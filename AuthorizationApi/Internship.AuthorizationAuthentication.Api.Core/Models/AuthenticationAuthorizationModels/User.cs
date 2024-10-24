using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Internship.AuthorizationAuthentication.Api.Core.Models.Enums;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;

public class User : IEntity
{
    [Column(TypeName = "varchar(50)")] 
    public string UserName { get; set; } = string.Empty;

    [EmailAddress] 
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    
    public string PasswordSalt { get; set; } = string.Empty;
    
    [DefaultValue(UserType.Student)]
    public UserType Role { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [EmailAddress]
    public string PersonalEmail { get; set; } = string.Empty;
}