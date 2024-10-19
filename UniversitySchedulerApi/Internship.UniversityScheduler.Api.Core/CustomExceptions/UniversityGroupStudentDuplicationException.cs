namespace Internship.UniversityScheduler.Api.Core.CustomExceptions;

public class UniversityGroupStudentDuplicationException : Exception
{
    public UniversityGroupStudentDuplicationException()
    {
        
    }

    public UniversityGroupStudentDuplicationException(string message) : base(message)
    {
        
    }

    public UniversityGroupStudentDuplicationException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}