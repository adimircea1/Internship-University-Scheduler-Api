using System.Linq.Expressions;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;

namespace Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;

public interface IRefreshTokenService
{
    public Task<RefreshToken?> GetRefreshTokenByQuery(Expression<Func<RefreshToken, bool>> query);
    public Task<RefreshToken> GetRefreshTokenByIdAsync(int refreshTokenId);
    public Task<List<RefreshToken>> GetAllRefreshTokensAsync();
    public Task<List<RefreshToken>> GetRefreshTokensByQuery(Expression<Func<RefreshToken, bool>> query);

    public Task AddRefreshTokenAsync(RefreshToken refreshToken);
    public Task DeleteRefreshTokenByIdAsync(int refreshTokenId);
    public Task DeleteRefreshTokenByUserIdAsync(int refreshTokenUserId);
    public Task DeleteAllRefreshTokensAsync();
    public Task DeleteRefreshTokenByValue(string refreshToken);

    public Task QueueAddRefreshTokenAsync(RefreshToken refreshToken);
    public Task QueueDeleteRefreshTokenByIdAsync(int refreshTokenId);
    public Task QueueDeleteRefreshTokenByUserIdAsync(int refreshTokenUserId);
    public void QueueDeleteAllRefreshTokens();
    public Task QueueDeleteRefreshTokenByValueAsync(string refreshToken);

}