using System.Text.Json.Serialization;

namespace OnEntitySharedLogic.Models;

[Newtonsoft.Json.JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderByDirection
{
    Ascending = 1,
    Descending = 2
}