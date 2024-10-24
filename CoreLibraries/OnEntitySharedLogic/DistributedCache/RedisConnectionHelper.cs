using OnEntitySharedLogic.CustomExceptions;
using StackExchange.Redis;

namespace OnEntitySharedLogic.DistributedCache;

public class RedisConnectionHelper : IRedisConnectionHelper
{
    private readonly RedisConfiguration _redisConfiguration;

    public RedisConnectionHelper(RedisConfiguration redisConfiguration)
    {
        _redisConfiguration = redisConfiguration;
    }

    public IDatabase GetDatabase()
    {
        return GetConnection().GetDatabase();
    }
    
    private ConnectionMultiplexer GetConnection()
    {
        var isDocker = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker";

        string connectionString;

        if (isDocker)
        {
            connectionString = Environment.GetEnvironmentVariable("Default__RedisConnectionString")
                               ?? throw new EntityNotFoundException("Could not find variable!");
        }
        else
        {
            connectionString =
                $"{_redisConfiguration.HostName}:{_redisConfiguration.Port},password={_redisConfiguration.Password},ssl={_redisConfiguration.Ssl}";
        }

        var redisConnectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

        return redisConnectionMultiplexer;
    }
}