using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Authenticators;

[Registration(Type = RegistrationKind.Scoped)]
public class UserAuthenticator : IUserAuthenticator
{
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly IRefreshTokenService _refreshTokenService;

    public UserAuthenticator(
        IRefreshTokenGenerator refreshTokenGenerator,
        IRefreshTokenService refreshTokenService,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _refreshTokenGenerator = refreshTokenGenerator;
        _refreshTokenService = refreshTokenService;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<TokenModel> AuthenticateAsync(User user)
    {
        var refreshToken = _refreshTokenGenerator.GenerateRefreshToken();

        await _refreshTokenService.AddRefreshTokenAsync(new RefreshToken
        {
            RefreshTokenValue = refreshToken,
            UserId = user.Id
        });

        return new TokenModel
        {
            AccessToken = _accessTokenGenerator.GenerateAccessToken(user),
            RefreshToken = refreshToken
        };
    }
}