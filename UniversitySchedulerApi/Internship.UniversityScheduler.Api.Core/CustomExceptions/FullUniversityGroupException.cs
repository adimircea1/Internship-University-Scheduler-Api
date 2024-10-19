namespace Internship.UniversityScheduler.Api.Core.CustomExceptions;

public class FullUniversityGroupException : Exception
{
    public FullUniversityGroupException()
    {
        
    }

    public FullUniversityGroupException(string message) : base(message)
    {
        
    }

    public FullUniversityGroupException(string message, Exception exception) : base(message, exception)
    {
        
    }
}