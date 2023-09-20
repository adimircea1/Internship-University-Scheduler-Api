namespace Internship.AuthorizationAuthentication.Api.Core.CustomExceptions;

public class UserClaimNotFoundException : Exception
{
    public UserClaimNotFoundException()
    {
        
    }

    public UserClaimNotFoundException(string message) : base(message)
    {
        
    }

    public UserClaimNotFoundException(string message, Exception exception) : base(message, exception)
    {
        
    }
}