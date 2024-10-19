using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;
using Microsoft.IdentityModel.Tokens;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.TokenGenerators;

[Registration(Type = RegistrationKind.Scoped)]
public class TokenGenerator : ITokenGenerator
{
    public string GenerateToken(string secretToken, string issuer, string audience, int expirationHours,
        IEnumerable<Claim>? claims = null)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretToken));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(expirationHours),
            credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}