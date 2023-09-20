namespace OnEntitySharedLogic.GRPC.Grpc_Setups;

public interface IGrpcClientService
{
    public TService GetService<TService>() where TService : class;
}