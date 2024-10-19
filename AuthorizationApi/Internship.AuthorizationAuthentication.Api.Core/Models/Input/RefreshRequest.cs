using System.ComponentModel.DataAnnotations;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.Input;

public class RefreshRequest
{
    [Required] 
    public string RefreshToken { get; set; } = string.Empty;
}