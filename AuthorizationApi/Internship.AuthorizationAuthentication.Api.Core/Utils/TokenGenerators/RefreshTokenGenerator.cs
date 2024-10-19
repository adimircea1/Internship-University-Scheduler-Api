using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Configuration;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.TokenGenerators;

[Registration(Type = RegistrationKind.Scoped)]
public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly AuthenticationConfiguration _authenticationConfiguration;
    private readonly ITokenGenerator _tokenGenerator;

    public RefreshTokenGenerator(ITokenGenerator tokenGenerator,
        AuthenticationConfiguration authenticationConfiguration)
    {
        _tokenGenerator = tokenGenerator;
        _authenticationConfiguration = authenticationConfiguration;
    }

    public string GenerateRefreshToken()
    {
        return _tokenGenerator.GenerateToken(
            _authenticationConfiguration.RefreshSecretToken,
            _authenticationConfiguration.Issuer,
            _authenticationConfiguration.Audience,
            _authenticationConfiguration.RefreshTokenExpirationTimeInHours
        );
    }
}