using OnEntitySharedLogic.Models;

namespace OnEntitySharedLogic.Utils;

[AttributeUsage(AttributeTargets.Class)]
public class RegistrationAttribute : Attribute
{
    public RegistrationKind Type { get; set; }
}