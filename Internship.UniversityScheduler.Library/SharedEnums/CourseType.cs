using System.Text.Json.Serialization;

namespace Internship.UniversityScheduler.Library.SharedEnums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CourseType
{
    Laboratory,
    Lecture,
    Seminary
}