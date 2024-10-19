using Grpc.Net.Client;

namespace OnEntitySharedLogic.GRPC.Grpc_Setups;

public interface IChannelManager
{
    public GrpcChannel GetChannel<TService>() where TService : class;
}