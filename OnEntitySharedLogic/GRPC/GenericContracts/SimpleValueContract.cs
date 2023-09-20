using ProtoBuf;

namespace OnEntitySharedLogic.GRPC.GenericContracts;

//This is a protobuf, a method for serializing structured data in gRPC
//Uses binary format
//This is the IDL (Definition language) used by gRPC
[ProtoContract]
public class SimpleValueContract<TType>
{
    [ProtoMember(1)]
    public TType? Value { get; set; } 
}