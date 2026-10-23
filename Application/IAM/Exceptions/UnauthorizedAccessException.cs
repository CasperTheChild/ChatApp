namespace Application.IAM.Exceptions;

internal class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException()
        : base("Unauthorized access") { }
}
