using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Internship.UniversityScheduler.Api.Presentation;

public static class Program
{
    //Here i will be creating a host, on which i will define some options (See CreateWebHostBuilder)
    //Kestrel is used to run the host

    public static async Task Main(string[] args)
    {
        var host = CreateWebHostBuilder(args).Build();
        await host.RunAsync();
    }

    private static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
        return WebHost
            .CreateDefaultBuilder(args)
            .ConfigureKestrel((_, options) =>
            {
                options.AllowSynchronousIO = true;
                options.Listen(IPAddress.Any, 5001, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
                
                options.Listen(IPAddress.Any, 5150, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                });
            })
            .UseStartup<Startup>();
    }
}