using Internship.UniversityScheduler.Library.SharedEnums;
using ProtoBuf;

namespace Internship.UniversityScheduler.Library.DataContracts;

[ProtoContract]
public class CourseDataContract
{
    [ProtoMember(1)]
    public int Id { get; set; }
    
    [ProtoMember(2)]
    public int NumberOfCredits { get; set; }
    
    [ProtoMember(3)]
    public int ProfessorId { get; set; }
    
    [ProtoMember(4)]
    public string CourseName { get; set; } = string.Empty;
    
    [ProtoMember(5)]
    public string Type { get; set; } = string.Empty;
    
    [ProtoMember(6)]
    public CourseDomain Domain { get; set; }
}