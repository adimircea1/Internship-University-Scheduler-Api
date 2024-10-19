using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace EmailVerificationApi.Presentation;

public static class Program
{
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
                options.Listen(IPAddress.Any, 5004, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
                
                options.Listen(IPAddress.Any, 5024, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http1;
                });
            })
            .UseStartup<Startup>();
    }
}