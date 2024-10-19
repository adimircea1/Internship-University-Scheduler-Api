using System.ComponentModel.DataAnnotations.Schema;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;

public class RefreshToken 
{
    public string RefreshTokenValue { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User? User { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}