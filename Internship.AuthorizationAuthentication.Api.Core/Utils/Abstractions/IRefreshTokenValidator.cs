namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

public interface IRefreshTokenValidator
{
    public bool Validate(string refreshToken);
}