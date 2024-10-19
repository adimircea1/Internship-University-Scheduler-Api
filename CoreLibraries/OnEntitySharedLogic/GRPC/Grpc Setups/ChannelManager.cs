using System.Reflection;
using Grpc.Net.Client;
using OnEntitySharedLogic.GRPC.Utils;

namespace OnEntitySharedLogic.GRPC.Grpc_Setups;

public class ChannelManager : IChannelManager
{
    //Map channels to a system type
    private Dictionary<SystemType, GrpcChannel?> ChannelMappings { get; set; } = new()
    {
        {
            SystemType.UniversityScheduler, null
        },

        {
            SystemType.AuthorizationAuthentication, null
        },

        {
            SystemType.StudentExamination, null
        },

        {
            SystemType.EmailVerification, null
        }
    };

    //These hosts will work with HTTP2
    private Dictionary<SystemType, string> SystemHosts { get; set; }

    public ChannelManager()
    {
        var dockerEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker";
        
        SystemHosts = new Dictionary<SystemType, string>
        {
            { SystemType.UniversityScheduler, dockerEnvironment ? "http://university-scheduler-api:5001" : "http://localhost:5001" },
            { SystemType.AuthorizationAuthentication, dockerEnvironment ? "http://authorization-authentication-api:5002" : "http://localhost:5002" },
            { SystemType.StudentExamination, dockerEnvironment ? "http://student-examination-api:5003" : "http://localhost:5003" },
            { SystemType.EmailVerification, dockerEnvironment ? "http://email-verification-api:5004" : "http://localhost:5004" },
        };
        
        
        SetChannelMappings();
    }
    
    //This returns the Grpc channel of the specified service
    public GrpcChannel GetChannel<TService>() where TService : class
    {
        var serviceType = typeof(TService);
        
        if (serviceType.GetCustomAttribute(typeof(ServiceTargetAttribute)) is not ServiceTargetAttribute serviceTargetAttribute)
        {
            throw new Exception("Non existent target for service!");
        }

        return ChannelMappings[serviceTargetAttribute.SystemType]!;
    }
    
    private void InitialiseChannel(SystemType systemType)
    {
        if (!ChannelMappings.ContainsKey(systemType))
        {
            throw new Exception("No channel configured for this system type!");
        }

        var channel = ChannelMappings[systemType];

        if (channel != null)
        {
            return;
        }

        var host = SystemHosts[systemType];
        
        //create channel
        channel = GrpcChannel.ForAddress(host, new GrpcChannelOptions
        {
            //add socket option
            //A socket enables program to establish connections and exchange different data between machines
            HttpHandler = new SocketsHttpHandler
            {
                //won't close the connection after a request is completed
                //enables http client to make multiple http2 connections to the same host
                EnableMultipleHttp2Connections = true
            }
        });

        ChannelMappings[systemType] = channel;
    }
    
    private void SetChannelMappings()
    {
        InitialiseChannel(SystemType.UniversityScheduler);
        InitialiseChannel(SystemType.AuthorizationAuthentication);
        InitialiseChannel(SystemType.StudentExamination);
        InitialiseChannel(SystemType.EmailVerification);
    }
}