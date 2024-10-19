using System.Text;
using System.Text.Json.Serialization;
using AutoMapper;
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
using StudentExamination.Api.Core.ExceptionHandlingMiddleware;
using StudentExamination.Api.Core.Utils.AutoMapper;
using StudentExamination.Api.Core.Utils.Configuration;
using StudentExamination.Api.Infrastructure.Repository;

namespace StudentExamination.Api.Presentation;

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

        services.AddSingleton(authenticationConfiguration);
        services.AddSingleton(mapper);
        
        services.AddScoped<IDataContext, DataContext>();
        
        services.InjectServices("StudentExamination.Api.Core");
        
        OnEntitySharedLogicUtilServicesInjection.InjectServices(services);
        GrpcContentInjection.InjectGrpcServices(services);

        services.AddGrpc();
        services.AddCodeFirstGrpc();
            
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        var isDocker = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Docker";
        var connectionString = isDocker ? Environment.GetEnvironmentVariable("ExaminationDockerDbConnectionString")
            : _configuration.GetConnectionString("ExaminationApiConnectionString");
        
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v3", new OpenApiInfo
            {
                Title = "My API",
                Version = "v3",
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
                    Array.Empty<string>()
                }
            });
        });

        services.AddCors(o => o.AddPolicy("FrontEndLocalhost", builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));

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
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v3/swagger.json", "My API v3");
        });

        loggerFactory.AddFile($@"{Directory.GetCurrentDirectory()}\Logs\log.txt");
        
        if (Environment.GetEnvironmentVariable("EnableStartupMigrations") != "true")
        {
            return;
        }

        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        serviceScope.ServiceProvider.GetService<DataContext>()!.Database.Migrate();
    }
}