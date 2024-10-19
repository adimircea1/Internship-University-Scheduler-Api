using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using OnEntitySharedLogic.CustomExceptions;

namespace Internship.AuthorizationAuthentication.Api.Core.ExceptionHandlingMiddleware;

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
    private static (HttpStatusCode code, string message) GetResponse(Exception exception)
    {
        HttpStatusCode statusCode;
        var message = "";
        switch (exception)
        {
            case EntityNotFoundException 
                or UserNotRegisteredException
                or UserClaimNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;

            case UserAlreadyRegisteredException
                or RegisterRequestAlreadyMadeException
                or WrongUserCredentialsException
                or InvalidNewPasswordException
                or DbUpdateException:
            {
                if (exception.InnerException is PostgresException postgresException)
                {
                    var sqlState = postgresException.SqlState;

                    switch (sqlState)
                    {
                        case "23505":
                            message = "Database duplication conflict";
                            break;
                            
                        case "22001": 
                            message = "Database validation conflict";
                            break;


                        case "23502": 
                            message = "Database null constraint violation";
                            break;


                        case "23503" : 
                            message = "Database foreign key conflict";
                            break;
                    }
                }

                //If i don't have Database exceptions, then the message will have the value of exception.Message itself
                if (message is "")
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
        if (message is "")
        {
            message = exception.Message;
        }

        return (statusCode, JsonSerializer.Serialize(message));
    }
}
