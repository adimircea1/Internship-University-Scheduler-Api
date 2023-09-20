namespace StudentExamination.Api.Core.CustomExceptions;

public class NullAccessTokenValue : Exception
{
    public NullAccessTokenValue()
    {
        
    }

    public NullAccessTokenValue(string message) : base(message)
    {
        
    }

    public NullAccessTokenValue(string message, Exception exception) : base(message, exception)
    {
        
    }
}