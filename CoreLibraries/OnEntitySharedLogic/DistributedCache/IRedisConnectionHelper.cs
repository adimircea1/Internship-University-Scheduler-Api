using StackExchange.Redis;

namespace OnEntitySharedLogic.DistributedCache;

public interface IRedisConnectionHelper
{
    public IDatabase GetDatabase();
}