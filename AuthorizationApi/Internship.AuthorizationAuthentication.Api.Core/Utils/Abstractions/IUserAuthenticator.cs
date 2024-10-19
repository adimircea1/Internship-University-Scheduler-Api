using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

public interface IUserAuthenticator
{
    public Task<TokenModel> AuthenticateAsync(User user);
}