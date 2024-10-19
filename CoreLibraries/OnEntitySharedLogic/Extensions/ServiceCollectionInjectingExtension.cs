using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OnEntitySharedLogic.Models;
using OnEntitySharedLogic.Utils;

namespace OnEntitySharedLogic.Extensions;

public static class ServiceCollectionInjectingExtension
{
    public static void InjectServices(this IServiceCollection services, string @namespace)
    {
        var assemblyToLoad = Assembly.Load(@namespace);

        var typesToInject = assemblyToLoad
            .GetTypes()
            .Where(type => type.GetCustomAttributes().OfType<RegistrationAttribute>().Any());

        foreach (var typeToInject in typesToInject)
        {
            var interfaceTypeToInject = typeToInject.GetInterfaces().First();

            var registrationAttribute = typeToInject.GetCustomAttribute(typeof(RegistrationAttribute)) as RegistrationAttribute;

            switch (registrationAttribute?.Type)
            {
                case RegistrationKind.Scoped:
                {
                    services.AddScoped(interfaceTypeToInject, typeToInject);
                    break;
                }

                case RegistrationKind.Singleton:
                {
                    services.AddSingleton(interfaceTypeToInject, typeToInject);
                    break;
                }

                case RegistrationKind.Transient:
                {
                    services.AddTransient(interfaceTypeToInject, typeToInject);
                    break;
                }

                default:
                    return;
            }
        }
    }
}