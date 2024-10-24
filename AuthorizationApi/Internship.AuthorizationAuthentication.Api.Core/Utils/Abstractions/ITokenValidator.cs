namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

public interface ITokenValidator
{
    public bool Validate(string refreshToken);
}