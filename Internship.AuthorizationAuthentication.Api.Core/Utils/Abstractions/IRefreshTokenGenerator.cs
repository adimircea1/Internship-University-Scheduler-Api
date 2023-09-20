namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

public interface IRefreshTokenGenerator
{
    public string GenerateRefreshToken();
}