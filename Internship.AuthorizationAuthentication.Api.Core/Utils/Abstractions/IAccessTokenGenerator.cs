using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

public interface IAccessTokenGenerator
{
    public string GenerateAccessToken(User user);
}