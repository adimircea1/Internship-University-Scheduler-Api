using Microsoft.Extensions.DependencyInjection;
using OnEntitySharedLogic.GRPC.Grpc_Setups;

namespace OnEntitySharedLogic.GRPC.Utils.InjectorClasses;

public static class GrpcContentInjection
{
    public static void InjectGrpcServices(IServiceCollection services)
    {
        services.AddScoped<IChannelManager, ChannelManager>();
        services.AddScoped<IGrpcClientService, GrpcClientService>();
    }
}