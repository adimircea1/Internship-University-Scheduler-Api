using System.Text.Json.Serialization;

namespace Internship.AuthorizationAuthentication.Api.Core.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    Admin,
    Professor,
    Student
}