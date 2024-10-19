namespace StudentExamination.Api.Core.CustomExceptions;

public class EmptyExamException : Exception
{
    public EmptyExamException()
    {
    }

    public EmptyExamException(string message) : base(message)
    {
    }

    public EmptyExamException(string message, Exception innerException) : base(message, innerException)
    {
    }
}