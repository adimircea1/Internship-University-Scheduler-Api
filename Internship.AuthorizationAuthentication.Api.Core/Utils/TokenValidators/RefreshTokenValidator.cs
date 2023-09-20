using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.TokenValidators;

[Registration(Type = RegistrationKind.Scoped)]
public class RefreshTokenValidator : IRefreshTokenValidator
{
    private readonly AuthenticationConfiguration _authenticationConfiguration;

    public RefreshTokenValidator(AuthenticationConfiguration authenticationConfiguration)
    {
        _authenticationConfiguration = authenticationConfiguration;
    }

    public bool Validate(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _authenticationConfiguration.Issuer,
            ValidAudience = _authenticationConfiguration.Audience,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfiguration.RefreshSecretToken)),
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            tokenHandler.ValidateToken(refreshToken, validationParameters, out _);
        }
        catch (Exception)
        {
            throw new ValidationException("The refresh token used for the request is not existent!");
        }
        
        return true;
    }
}