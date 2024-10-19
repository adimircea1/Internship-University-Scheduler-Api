namespace Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;

public class UserAlreadyRegisteredException : Exception
{
    public UserAlreadyRegisteredException()
    {
        
    }

    public UserAlreadyRegisteredException(string message) : base(message)
    {
        
    }

    public UserAlreadyRegisteredException(string message, Exception exception) : base(message, exception)
    {
        
    }
}