namespace Internship.AuthorizationAuthentication.Api.Core.Models.Utils;

public class PasswordData
{
    public string HashedPassword { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
}