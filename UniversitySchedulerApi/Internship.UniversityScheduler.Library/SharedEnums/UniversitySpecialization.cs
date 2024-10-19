using System.Text.Json.Serialization;

namespace Internship.UniversityScheduler.Library.SharedEnums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UniversitySpecialization
{
    AdvancedComputerScience,
    EconomicAndInformatics,
    Informatics
}