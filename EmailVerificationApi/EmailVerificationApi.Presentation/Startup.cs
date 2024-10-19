using System.Text.Json.Serialization;
using EmailVerificationApi.Core.Jobs;
using EmailVerificationApi.Core.Models;
using EmailVerificationApi.Core.Utils;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.GRPC.Utils.InjectorClasses;
using OnEntitySharedLogic.Utils;
using ProtoBuf.Grpc.Server;
using Quartz;

namespace EmailVerificationApi.Presentation;

public class Startup
{
    private readonly IConfiguration _configuration;
    
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        var smtpClientConfiguration = new SmtpClientConfiguration();
        _configuration.Bind("SmtpClientConfiguration", smtpClientConfiguration);
        
        var authenticationConfiguration = new AuthenticationConfiguration();
        _configuration.Bind("Authentication", authenticationConfiguration);
        
        services.AddSingleton(smtpClientConfiguration);
        services.AddSingleton(authenticationConfiguration);
        services.InjectServices("EmailVerificationApi.Core");
        services.InjectServices("EmailVerificationApi.Presentation");
        services.InjectServices("EmailVerification.Library");
        services.InjectRabbitMq();
        
        GrpcContentInjection.InjectGrpcServices(services);
        
        services.AddGrpc();
        services.AddCodeFirstGrpc();
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        
        services.AddCors(o => o.AddPolicy("FrontEndLocalhost", builder =>
        {
            builder
                .AllowAnyOrigin() //TO DO Remove this after further testing
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));
        
        services.AddQuartz(quartz =>
        {
            quartz.AddJobAndTrigger<StudentGradeEmailSender>(_configuration);
        });
        
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.UseCors("FrontEndLocalhost");
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapGrpcServices();
        });
        
        loggerFactory.AddFile($@"{Directory.GetCurrentDirectory()}\Logs\log.txt");
    }
}