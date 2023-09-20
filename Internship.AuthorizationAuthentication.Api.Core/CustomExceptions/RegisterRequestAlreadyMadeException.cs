namespace Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;

public class RegisterRequestAlreadyMadeException : Exception
{
    public RegisterRequestAlreadyMadeException()
    {
        
    }

    public RegisterRequestAlreadyMadeException(string message) : base(message)
    {
        
    }

    public RegisterRequestAlreadyMadeException(string message, Exception exception) : base(message, exception)
    {
        
    }
}