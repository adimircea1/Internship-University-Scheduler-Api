using Microsoft.AspNetCore.Http;

namespace OnEntitySharedLogic.Services;

[Obsolete]
public interface IExternalEntityService
{
    public Task<T> GetExternalEntityAsync<T>(string requestUri, HttpClient httpClient, IHttpContextAccessor httpContextAccessor);
    public Task AddExternalEntityAsync<T>(T entityToPost, string requestUri, HttpClient httpClient, IHttpContextAccessor httpContextAccessor);
}