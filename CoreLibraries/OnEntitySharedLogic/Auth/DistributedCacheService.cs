using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OnEntitySharedLogic.Utils;
using StackExchange.Redis;

namespace OnEntitySharedLogic.Auth;

public class DistributedCacheService : IDistributedCacheService
{
    private readonly IDatabase _redisDb;
    private readonly ILogger<DistributedCacheService> _logger;

    public DistributedCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<DistributedCacheService> logger)
    {
        _logger = logger;
        _redisDb = connectionMultiplexer.GetDatabase();
    }

    public async Task<DynamicResponse> AddAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        try
        {
            var jsonValue = JsonConvert.SerializeObject(value);
            var isAdded = await _redisDb.StringSetAsync(key, jsonValue, expiry);

            return isAdded 
                ? new DynamicResponse(204, "Added item") 
                : new DynamicResponse(409, "Failed to add item");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return new DynamicResponse(500, "An unhanled exception occured");
        }
    }

    public async Task<DynamicResponse> UpdateAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        try
        {
            var exists = await ExistsAsync(key);
            if (!exists)
            {
                return new DynamicResponse(404, "Item not found in cache.");
            }

            var serializedValue = JsonConvert.SerializeObject(value);
            var isUpdated = await _redisDb.StringSetAsync(key, serializedValue, expiry);

            return isUpdated 
                ? new DynamicResponse(201, "Item updated") 
                : new DynamicResponse(500, "Failed to update item");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return new DynamicResponse(500, "An unhanled exception occured");
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _redisDb.StringGetAsync(key);
            return value.IsNullOrEmpty
                ? default
                : JsonConvert.DeserializeObject<T>(value!);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return default;
        }
    }

    public async Task<DynamicResponse> RemoveAsync(string key)
    {
        try
        {
            var isRemoved = await _redisDb.KeyDeleteAsync(key);

            return isRemoved 
                ? new DynamicResponse(201, "Removed item") 
                : new DynamicResponse(409, "Failed to remove item");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return new DynamicResponse(500, "An unhanled exception occured");
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            return await _redisDb.KeyExistsAsync(key);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message);
            return false;
        }
    }
}