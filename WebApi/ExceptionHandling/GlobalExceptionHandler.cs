using Application.Conversations.Exceptions;
using Application.Messages.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        var (statusCode, message) = exception switch
        {
            ConversationNotFoundException =>
                (StatusCodes.Status404NotFound,
                 "Conversation not found"),

            ParticipantAlreadyExistsException =>
                (StatusCodes.Status409Conflict,
                 "Participant already exists"),

            ParticipantNotFoundException =>
                (StatusCodes.Status404NotFound,
                 "Participant not found"),

            MessageNotFoundException =>
                (StatusCodes.Status404NotFound,
                 "Message not found"),

            UnauthorizedAccessException =>
                (StatusCodes.Status401Unauthorized,
                 "Unauthorized access"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "An unexpected error occurred")
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new { error = message },
            cancellationToken);

        return true;
    }
}
