using Internship.AuthorizationAuthentication.Api.Core.Models.Enums;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.GetDtos;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserType Role { get; set; }
    public bool VerifiedEmail { get; set; } 
}