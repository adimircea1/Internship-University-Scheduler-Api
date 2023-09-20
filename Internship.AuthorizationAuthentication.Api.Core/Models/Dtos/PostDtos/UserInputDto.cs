using Internship.AuthorizationAuthentication.Api.Core.Models.Enums;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.Dtos.PostDtos;

public class UserInputDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PersonalEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserType Role { get; set; }
}