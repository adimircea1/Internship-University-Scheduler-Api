namespace Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;

public class TokenModel
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}