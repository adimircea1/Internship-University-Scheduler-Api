namespace Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;

public class UserNotRegisteredException : Exception
{
    public UserNotRegisteredException()
    {
        
    }

    public UserNotRegisteredException(string message) : base(message)
    {
        
    }

    public UserNotRegisteredException(string message, Exception exception) : base(message, exception)
    {
        
    }
}