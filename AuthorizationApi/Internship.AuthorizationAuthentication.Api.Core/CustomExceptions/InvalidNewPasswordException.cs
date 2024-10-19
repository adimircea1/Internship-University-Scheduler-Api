namespace Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;

public class InvalidNewPasswordException : Exception
{
    public InvalidNewPasswordException()
    {
        
    }

    public InvalidNewPasswordException(string message) : base(message)
    {
        
    }

    public InvalidNewPasswordException(string message, Exception exception) : base(message, exception)
    {
        
    }
}