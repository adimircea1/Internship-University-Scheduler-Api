namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Configuration;

public class AuthenticationConfiguration
{
    public string SecretToken { get; set; } = string.Empty;
    public string RefreshSecretToken { get; set; } = string.Empty;
    public int TokenExpirationTimeInHours { get; set; }
    public int RefreshTokenExpirationTimeInHours { get; set; }
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}