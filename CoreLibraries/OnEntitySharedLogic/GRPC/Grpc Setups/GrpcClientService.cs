using ProtoBuf.Grpc.Client;

namespace OnEntitySharedLogic.GRPC.Grpc_Setups;

//A gRPC client service scope is to push data through a system channel
public class GrpcClientService : IGrpcClientService
{
    private readonly IChannelManager _channelManager;

    public GrpcClientService(IChannelManager channelManager)
    {
        _channelManager = channelManager;
    }

    public TService GetService<TService>() where TService : class
    {
        //Get the service channel through which data is gonna be pushed
        var channel = _channelManager.GetChannel<TService>();
        
        //Create a gRPC service, which utilizes the above channel, for the TService type
        var service = channel.CreateGrpcService<TService>();

        return service;
    }
}