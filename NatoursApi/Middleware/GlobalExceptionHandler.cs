using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NatoursApi.Domain.Exceptions;

namespace NatoursApi.Middleware;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        logger.LogError("💥 Something went wrong: {error}", exception.Message);

        var result = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = "💥 An error occurred.",
                Status = httpContext.Response.StatusCode,
                Detail = exception.Message,
                Type = exception.GetType().Name
            },
            Exception = exception
        });

        if (!result)
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "💥 An error occurred.",
                Status = httpContext.Response.StatusCode,
                Detail = exception.Message,
                Type = exception.GetType().Name
            }, cancellationToken);

        return true;
    }
}