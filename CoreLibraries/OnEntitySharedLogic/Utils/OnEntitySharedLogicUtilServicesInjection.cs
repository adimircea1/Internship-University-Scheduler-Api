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
        services.AddScoped<IDistributedCacheService, DistributedCacheService>();

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
    }

    public static void InjectRedis(IServiceCollection services)
    {
        var redis = new RedisConfiguration();
        services.AddSingleton(redis);
        
    }
}