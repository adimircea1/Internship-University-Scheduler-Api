using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using Internship.UniversityScheduler.Api.Core.BackgroundTasks;
using Internship.UniversityScheduler.Api.Core.ExceptionHandlingMiddleware;
using Internship.UniversityScheduler.Api.Core.Utils.AutoMapper;
using Internship.UniversityScheduler.Api.Core.Utils.Configuration;
using Internship.UniversityScheduler.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.GRPC.Utils.InjectorClasses;
using OnEntitySharedLogic.Utils;
using ProtoBuf.Grpc.Server;
using Quartz;

namespace Internship.UniversityScheduler.Api.Presentation;

public class Startup
{
    private readonly IConfiguration _configuration;
    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var authenticationConfiguration = new AuthenticationConfiguration();
        _configuration.Bind("Authentication", authenticationConfiguration);

        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile(new MappingProfile());
        });
        var mapper = mapperConfiguration.CreateMapper();
        services.AddSingleton(mapper);
        services.AddSingleton(authenticationConfiguration);
        
        services.InjectServices("Internship.UniversityScheduler.Api.Presentation");
        services.InjectServices("Internship.UniversityScheduler.Api.Core");
        services.InjectServices("Internship.UniversityScheduler.Api.Infrastructure");
        services.InjectRabbitMq();
        services.AddScoped<IDataContext, DataContext>();
        
        GrpcContentInjection.InjectGrpcServices(services);
        OnEntitySharedLogicUtilServicesInjection.InjectServices(services);
        
        services.AddGrpc();
        services.AddCodeFirstGrpc();
        services.AddHttpContextAccessor();
        
        var isDocker = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker";
        var connectionString = isDocker ? Environment.GetEnvironmentVariable("UniversitySchedulerDockerDbConnectionString")
            : _configuration.GetConnectionString("UniversitySchedulerDbConnectionString");
        
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "University Scheduler Api",
                Version = "v1",
                Description = "A simple ASP.NET Core Web API for university management"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token!",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddCors(o => o.AddPolicy("FrontEndLocalhost", builder =>
        {
            builder
                .AllowAnyOrigin() //TO DO Remove this after further testing
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.SecretToken)),
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();
        
        services.AddQuartz(quartz =>
        {
            quartz.AddJobAndTrigger<StudentGradeDataDistribution>(_configuration);
        });
        
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    }

    public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
    {
        app.UseCors("FrontEndLocalhost");
        app.UseSwagger();
        app.UseRouting();
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next();
        });
        app.UseMiddleware<CaptureRequestBodyMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapGrpcServices();

        });
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
        });
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        loggerFactory.AddFile($@"{Directory.GetCurrentDirectory()}\Logs\log.txt");
        
        if (Environment.GetEnvironmentVariable("EnableStartupMigrations") != "true")
        {
            return;
        }

        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        serviceScope.ServiceProvider.GetService<DataContext>()!.Database.Migrate();

    }
}