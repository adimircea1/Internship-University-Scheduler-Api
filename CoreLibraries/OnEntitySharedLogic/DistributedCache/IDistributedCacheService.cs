using OnEntitySharedLogic.Utils;

namespace OnEntitySharedLogic.DistributedCache;

public interface IDistributedCacheService
{
    public Task<DynamicResponse> AddAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<DynamicResponse> UpdateAsync<T>(string key, T value, TimeSpan? expiry = null);
    public Task<T?> GetAsync<T>(string key);
    public Task<DynamicResponse> RemoveAsync(string key);
    public Task<bool> ExistsAsync(string key);
}
