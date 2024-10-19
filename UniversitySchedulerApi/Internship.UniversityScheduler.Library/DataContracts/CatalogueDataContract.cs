using ProtoBuf;

namespace Internship.UniversityScheduler.Library.DataContracts;

[ProtoContract]
public class CatalogueDataContract
{
    [ProtoMember(1)]
    public int Id { get; set; }
    
    [ProtoMember(2)]
    public int UniversityGroupId { get; set; }
}