using Internship.UniversityScheduler.Library.SharedEnums;
using ProtoBuf;

namespace Internship.UniversityScheduler.Library.DataContracts;

[ProtoContract]
public class ProfessorInputDataContract
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
    public ProfessorSpeciality Speciality { get; set; }
}