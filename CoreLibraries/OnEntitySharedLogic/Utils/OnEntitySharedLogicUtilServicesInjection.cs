using Microsoft.Extensions.DependencyInjection;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.DistributedCache;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;

namespace OnEntitySharedLogic.Utils;

public class OnEntitySharedLogicUtilServicesInjection
{
    public static void InjectServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IDatabaseGenericRepository<>), typeof(DatabaseGenericRepository<>));
        services.AddScoped<IExpressionBuilder, ExpressionBuilder>();
        services.AddScoped<IDistributedCacheService, DistributedCacheService>();
    }
}