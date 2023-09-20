namespace Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;

public class ChangePasswordOptions
{
    public string NewPassword { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
}