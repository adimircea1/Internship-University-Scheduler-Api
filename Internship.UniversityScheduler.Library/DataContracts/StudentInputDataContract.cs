using ProtoBuf;

namespace Internship.UniversityScheduler.Library.DataContracts;

[ProtoContract]
public class StudentInputDataContract
{
    [ProtoMember(1)] 
    public string FirstName { get; set; } = string.Empty;

    [ProtoMember(2)] 
    public string LastName { get; set; } = string.Empty;
    
    [ProtoMember(3)]
    public string PhoneNumber { get; set; } = string.Empty;

    [ProtoMember(4)]
    public string Email { get; set; } = string.Empty;
    
    [ProtoMember(5)]
    public DateTime BirthdayDate { get; set; }
    
    [ProtoMember(6)]
    public int StudyYear { get; set; }

    [ProtoMember(7)] 
    public string PersonalEmail { get; set; } = string.Empty;
}