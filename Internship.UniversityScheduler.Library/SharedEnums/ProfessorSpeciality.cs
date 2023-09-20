using System.Text.Json.Serialization;

namespace Internship.UniversityScheduler.Library.SharedEnums;

//this attribute will help me get enum values from requests
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProfessorSpeciality
{
    Backend,
    ComputerScience,
    Dotnet,
    Frontend,
    Maths
}