using System.ComponentModel.DataAnnotations;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.Input;

public class LoginRequest
{
    [Required] 
    public string Username { get; set; } = string.Empty;

    [Required] 
    public string Password { get; set; } = string.Empty;
}