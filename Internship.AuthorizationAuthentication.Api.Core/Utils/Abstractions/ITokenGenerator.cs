using System.Security.Claims;

namespace Internship.AuthorizationAuthentication.Api.Core.Utils.Abstractions;

public interface ITokenGenerator
{
    public string GenerateToken(string secretToken, string issuer, string audience, int expirationHours,
        IEnumerable<Claim>? claims = null);
}