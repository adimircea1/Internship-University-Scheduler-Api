using ProtoBuf;

namespace Internship.UniversityScheduler.Library.DataContracts;

[ProtoContract]
public class GradeInputDataContract
{
    [ProtoMember(1)]
    public int CatalogueId { get; set; }
    
    [ProtoMember(2)]
    public int CourseId { get; set; }
    
    [ProtoMember(3)]
    public int StudentId { get; set; }
    
    [ProtoMember(4)]
    public int Value { get; set; }
    
    [ProtoMember(5)]
    public DateTime DateOfGrade { get; set; }
}