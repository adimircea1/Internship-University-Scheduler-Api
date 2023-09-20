using Microsoft.Extensions.DependencyInjection;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Services.LambdaExpressionCreator;

namespace OnEntitySharedLogic.GRPC.Utils.InjectorClasses;

public class OnEntitySharedLogicUtilServicesInjection
{
    public static void InjectServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IDatabaseGenericRepository<>), typeof(DatabaseGenericRepository<>));
        services.AddScoped<IExpressionBuilder, ExpressionBuilder>();
    }
}