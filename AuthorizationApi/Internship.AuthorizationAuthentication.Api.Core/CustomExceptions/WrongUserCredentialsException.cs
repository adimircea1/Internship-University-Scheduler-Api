namespace Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;

public class WrongUserCredentialsException : Exception
{
    public WrongUserCredentialsException()
    {
        
    }

    public WrongUserCredentialsException(string message) : base(message)
    {
        
    }

    public WrongUserCredentialsException(string message, Exception exception) : base(message, exception)
    {
        
    }
}