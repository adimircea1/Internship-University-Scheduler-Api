using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using ProtoBuf.Grpc.Configuration;

namespace OnEntitySharedLogic.Extensions;

public static class GrpcServiceEndpointMapper
{
    public static void MapGrpcServices(this IEndpointRouteBuilder endpoints)
    {
        //Get project assemblies
        var projectAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        //Get grpc interfaces
        var grpcInterfaces = projectAssemblies.SelectMany(assembly => assembly.GetTypes()
            .Where(type => type.GetCustomAttribute(typeof(ServiceAttribute)) is ServiceAttribute));

        //grpcInterface from grpcInterfaces is a type variable known at runtime, but we need it at compile time
        //We cannot directly use endpoint.MapGrpcService<grpcInterface>, so for each interface in grpcInterfaces, we will need to invoke this method
        var grpcEndpointMappingMethod = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod("MapGrpcService");
        
        foreach (var grpcInterface in grpcInterfaces)
        {
            var genericMappingMethod = grpcEndpointMappingMethod!.MakeGenericMethod(grpcInterface);
            genericMappingMethod.Invoke(null, new object?[] { endpoints });
        }
    }
}
