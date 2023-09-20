namespace StudentExamination.Api.Core.CustomExceptions;

public class DuplicateExamGenerationException : Exception
{
    public DuplicateExamGenerationException()
    {
    }

    public DuplicateExamGenerationException(string message) : base(message)
    {
    }

    public DuplicateExamGenerationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}