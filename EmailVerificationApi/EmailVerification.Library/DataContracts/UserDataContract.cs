using ProtoBuf;

namespace EmailVerification.Library.DataContracts;

[ProtoContract]
public class UserDataContract
{
    [ProtoMember(1)]
    public string ReceiverEmail { get; set; } = string.Empty;
    
    [ProtoMember(2)]
    public string TemporaryPassword { get; set; } = string.Empty;
    
    [ProtoMember(3)]
    public string UserName { get; set; } = string.Empty;
}