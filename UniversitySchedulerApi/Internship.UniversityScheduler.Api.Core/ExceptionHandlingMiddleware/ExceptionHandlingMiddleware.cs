using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Grpc.Core;
using Internship.UniversityScheduler.Api.Core.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using OnEntitySharedLogic.CustomExceptions;

namespace Internship.UniversityScheduler.Api.Core.ExceptionHandlingMiddleware;

//Here i will handle all my exceptions
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var (status, message) = GetResponse(exception);
            response.StatusCode = (int)status;
            
            _logger.LogError(exception, $"{DateTime.Now} ---> {message}");
            await response.WriteAsync(message);
        }
    }

    //This will return a tuple (code, message)
    private (HttpStatusCode code, string message) GetResponse(Exception exception)
    {
        HttpStatusCode statusCode;
        var message = string.Empty;
        switch (exception)
        {
            case EntityNotFoundException :
                statusCode = HttpStatusCode.NotFound;
                break;

            case EmptyUniversityGroupException
                or UniversityGroupStudentDuplicationException
                or DbUpdateException
                or RpcException
                or FullUniversityGroupException:
            {
                if (exception.InnerException is PostgresException postgresException)
                {
                    var sqlState = postgresException.SqlState;

                    message = sqlState switch
                    {
                        "23505" => "Database duplication conflict",
                        "22001" => "Database validation conflict",
                        "23502" => "Database null constraint violation",
                        "23503" => "Database foreign key conflict",
                        _ => message
                    };
                }

                //If i don't have Database exceptions, then the message will have the value of exception.Message itself
                if (string.IsNullOrEmpty(message))
                {
                    message = exception.Message;
                }
                statusCode = HttpStatusCode.Conflict;
            }
                break;
            
            case ValidationException:
                statusCode = HttpStatusCode.BadRequest;
                break;
            
            default:
                statusCode = HttpStatusCode.InternalServerError;
                break;
        }

        //I have custom messages for the Database exceptions
        if (string.IsNullOrEmpty(message))
        {
            message = exception.Message;
        }

        return (statusCode, JsonSerializer.Serialize(message));
    }
}

