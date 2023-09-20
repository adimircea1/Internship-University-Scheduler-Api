namespace Internship.UniversityScheduler.Api.Core.CustomExceptions;

public class EmptyUniversityGroupException : Exception
{
    public EmptyUniversityGroupException()
    {
        
    }

    public EmptyUniversityGroupException(string message) : base(message)
    {
        
    }

    public EmptyUniversityGroupException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}