namespace Internship.AuthorizationAuthentication.Api.Core.Models.Utils;

public class UserCredentials
{
    public string UserName { get; set; } = string.Empty;
    public string UserUniversityEmail { get; set; } = string.Empty;
    public string TemporaryPassword { get; set; } = string.Empty;
}