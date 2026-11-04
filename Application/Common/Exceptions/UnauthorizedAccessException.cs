namespace Application.Common.Exceptions;

internal class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException()
        : base("Unauthorized access") { }
}
