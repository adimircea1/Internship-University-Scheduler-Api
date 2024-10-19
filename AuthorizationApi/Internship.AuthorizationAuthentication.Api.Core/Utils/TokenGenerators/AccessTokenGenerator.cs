using System.Security.Claims;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Configuration;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.TokenGenerators;

[Registration(Type = RegistrationKind.Scoped)]
public class AccessTokenGenerator : IAccessTokenGenerator
{
    private readonly AuthenticationConfiguration _authenticationConfiguration;
    private readonly ITokenGenerator _tokenGenerator;

    public AccessTokenGenerator(AuthenticationConfiguration authenticationConfiguration,
        ITokenGenerator tokenGenerator)
    {
        _authenticationConfiguration = authenticationConfiguration;
        _tokenGenerator = tokenGenerator;
    }

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new("Role", user.Role.ToString()),
            new( ClaimTypes.Email, user.Email)
        };

        return _tokenGenerator.GenerateToken(
            _authenticationConfiguration.SecretToken,
            _authenticationConfiguration.Issuer,
            _authenticationConfiguration.Audience,
            _authenticationConfiguration.TokenExpirationTimeInHours,
            claims);
    }
}