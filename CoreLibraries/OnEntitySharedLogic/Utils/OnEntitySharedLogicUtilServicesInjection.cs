using Microsoft.Extensions.DependencyInjection;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.DistributedCache;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;
using StackExchange.Redis;

namespace OnEntitySharedLogic.Utils;

public class OnEntitySharedLogicUtilServicesInjection
{
    public static void InjectServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IDatabaseGenericRepository<>), typeof(DatabaseGenericRepository<>));
        services.AddScoped<IExpressionBuilder, ExpressionBuilder>();
    }

    public static void InjectRedis(IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
        services.AddScoped<IRedisConnectionHelper, RedisConnectionHelper>();
        var redis = new RedisConfiguration();
        services.AddSingleton(redis);
        services.AddScoped<IDistributedCacheService, DistributedCacheService>();
    }
}