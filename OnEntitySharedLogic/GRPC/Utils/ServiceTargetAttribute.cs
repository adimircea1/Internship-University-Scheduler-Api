namespace OnEntitySharedLogic.GRPC.Utils;

//This attribute is used to mark gRPC services
public class ServiceTargetAttribute : Attribute
{
    public SystemType SystemType { get; set; }
}