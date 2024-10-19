using EmailVerification.Library.DataContracts;
using OnEntitySharedLogic.GRPC.Utils;
using ProtoBuf.Grpc.Configuration;

namespace EmailVerification.Library.GrpcServiceInterfaces;

[Service]
[ServiceTarget(SystemType = SystemType.EmailVerification)]
public interface ICredentialsGrpcService
{
    [Operation]
    public ValueTask SendUserCredentialsAsync(UserDataContract userData);
}