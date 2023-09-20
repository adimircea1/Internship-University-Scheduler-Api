using ProtoBuf;

namespace Internship.UniversityScheduler.Library.DataContracts;

[ProtoContract]
public class StudentDataContract
{
    [ProtoMember(1)]
    public int Id { get; set; }
    
    [ProtoMember(2)]
    public int StudyYear { get; set; }
    
    [ProtoMember(3)]
    public string FirstName { get; set; } = string.Empty;
    
    [ProtoMember(4)]
    public string LastName { get; set; } = string.Empty;
    
    [ProtoMember(5)]
    public string FullName { get; set; } = string.Empty;

    [ProtoMember(6)] 
    public DateTime BirthdayDate { get; set; } 
    
    [ProtoMember(7)]
    public string Email { get; set; } = string.Empty;
    
    [ProtoMember(8)]
    public string? PhoneNumber { get; set; }
    
    [ProtoMember(9)]
    public int? UniversityGroupId { get; set; }
}