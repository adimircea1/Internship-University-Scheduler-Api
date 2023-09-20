using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
using Internship.AuthorizationAuthentication.Api.Core.ExceptionHandlingMiddleware;
using Internship.AuthorizationAuthentication.Api.Core.Utils.AutoMapper;
using Internship.AuthorizationAuthentication.Api.Core.Utils.Configuration;
using Internship.AuthorizationAuthentication.Api.Infrastructure;
using Internship.AuthorizationAuthentication.Api.Infrastructure.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnEntitySharedLogic.DatabaseGenericRepository;
using OnEntitySharedLogic.Extensions;
using OnEntitySharedLogic.GRPC.Utils.InjectorClasses;
using OnEntitySharedLogic.Utils;
using ProtoBuf.Grpc.Server;

namespace Internship.AuthorizationAuthentication.Api.Presentation;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Startup> _logger;
    public Startup(
        IConfiguration configuration, 
        ILogger<Startup> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var authenticationConfiguration = new AuthenticationConfiguration();
        _configuration.Bind("Authentication", authenticationConfiguration);
        services.AddSingleton(authenticationConfiguration);
        
        var mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile(new MappingProfile(new PasswordManager()));
        });
        var mapper = mapperConfiguration.CreateMapper();
        services.AddSingleton(mapper);
        
        services.InjectServices("Internship.AuthorizationAuthentication.Api.Infrastructure");
        services.InjectServices("Internship.AuthorizationAuthentication.Api.Core");
        services.AddScoped<IDataContext, DataContext>();
        OnEntitySharedLogicUtilServicesInjection.InjectServices(services);
        GrpcContentInjection.InjectGrpcServices(services);
        
        services.AddGrpc();
        services.AddCodeFirstGrpc();
        services.AddHttpContextAccessor();
        
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        
        var isDocker = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker";
        var connectionString = isDocker ? Environment.GetEnvironmentVariable("AuthorizationAuthenticationDockerDbConnectionString")
            : _configuration.GetConnectionString("AuthorizationAuthenticationDbConnectionStringLocalHost");
        
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "My API",
                Version = "v2",
                Description = "A simple ASP.NET Core Web API"
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
                    new string[] { }
                }
            });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.SecretToken)),
                ClockSkew = TimeSpan.Zero
            };
        });
        
        services.AddCors(o => o.AddPolicy("FrontEndLocalhost", builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));
        
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
        });
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API v2"); });

        loggerFactory.AddFile($@"{Directory.GetCurrentDirectory()}\Logs\log.txt");

        if (Environment.GetEnvironmentVariable("EnableStartupMigrations") != "true")
        {
            return;
        }

        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        serviceScope.ServiceProvider.GetService<DataContext>()!.Database.Migrate();
    }
}