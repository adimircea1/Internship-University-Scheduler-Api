using System.Linq.Expressions;
using Internship.AuthorizationAuthentication.Api.Core.Models.AuthenticationAuthorizationModels;
using Internship.AuthorizationAuthentication.Api.Core.Services.Abstractions;
using Microsoft.Extensions.Logging;
using OnEntitySharedLogic.CustomExceptions;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace Internship.AuthorizationAuthentication.Api.Core.Services;

[Registration(Type = RegistrationKind.Scoped)]
public class RefreshTokenService : IRefreshTokenService
{
    private readonly IDatabaseGenericRepository<RefreshToken> _refreshTokenRepository;
    private readonly ILogger<RefreshTokenService> _logger;

    public RefreshTokenService(
        IDatabaseGenericRepository<RefreshToken> refreshTokenRepository,
        ILogger<RefreshTokenService> logger)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<RefreshToken?> GetRefreshTokenByQuery(Expression<Func<RefreshToken, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving refresh token by query has been made!");
        return await _refreshTokenRepository.GetEntityByQueryAsync(query);
    }

    public async Task<RefreshToken> GetRefreshTokenByIdAsync(int refreshTokenId)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving refresh token by id {refreshTokenId} has been made!");
        return await _refreshTokenRepository.GetEntityByQueryAsync(refreshToken => refreshToken.Id == refreshTokenId)
               ?? throw new EntityNotFoundException($"Cannot find refresh token with id {refreshTokenId}!");
    }

    public async Task<List<RefreshToken>> GetAllRefreshTokensAsync()
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving all refresh tokens has been made!");
        return await _refreshTokenRepository.GetAllEntitiesAsync();
    }

    public async Task<List<RefreshToken>> GetRefreshTokensByQuery(Expression<Func<RefreshToken, bool>> query)
    {
        _logger.LogInformation($"{DateTime.Now} ---> An attempt of retrieving all refresh tokens by query has been made!");
        return await _refreshTokenRepository.GetEntitiesByQueryAsync(query);
    }

    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await QueueAddRefreshTokenAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully added refresh token to database!");
    }

    public async Task DeleteRefreshTokenByIdAsync(int refreshTokenId)
    {
        await QueueDeleteRefreshTokenByIdAsync(refreshTokenId);
        await _refreshTokenRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully removed refresh token from database!");
    }

    public async Task DeleteRefreshTokenByUserIdAsync(int refreshTokenUserId)
    {
        await QueueDeleteRefreshTokenByUserIdAsync(refreshTokenUserId);
        await _refreshTokenRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully removed refresh token with user id {refreshTokenUserId} from database!");
    }

    public async Task DeleteAllRefreshTokensAsync()
    {
        QueueDeleteAllRefreshTokens();
        await _refreshTokenRepository.SaveChangesAsync();
    }

    public async Task DeleteRefreshTokenByValue(string refreshToken)
    {
        await QueueDeleteRefreshTokenByValueAsync(refreshToken);
        await _refreshTokenRepository.SaveChangesAsync();
        _logger.LogInformation($"{DateTime.Now} ---> Successfully removed refresh token from database!");
    }

    public async Task QueueAddRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _refreshTokenRepository.AddEntityAsync(refreshToken);
    }

    public async Task QueueDeleteRefreshTokenByIdAsync(int refreshTokenId)
    {
        var existingRefreshToken = await GetRefreshTokenByIdAsync(refreshTokenId);
        _refreshTokenRepository.DeleteEntity(existingRefreshToken);
    }

    public async Task QueueDeleteRefreshTokenByUserIdAsync(int refreshTokenUserId)
    {
        var refreshTokenToDelete = await GetRefreshTokenByQuery(token => token.UserId == refreshTokenUserId) ??
                                   throw new EntityNotFoundException($"Cannot delete refresh token - refresh token of the user with id {refreshTokenUserId} is not existent!");
        _refreshTokenRepository.DeleteEntity(refreshTokenToDelete);
    }

    public void QueueDeleteAllRefreshTokens()
    {
        _refreshTokenRepository.DeleteAllEntities();
    }

    public async Task QueueDeleteRefreshTokenByValueAsync(string refreshToken)
    {
        var existingRefreshTokenByValue = await _refreshTokenRepository.GetEntityByQueryAsync(token => token.RefreshTokenValue == refreshToken) ??
                                          throw new EntityNotFoundException($"Cannot delete refresh token - refresh token not existent!");
        _refreshTokenRepository.DeleteEntity(existingRefreshTokenByValue);
    }
}