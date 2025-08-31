using Microsoft.AspNetCore.Http;

namespace NatoursApi.Domain.Exceptions;

public abstract class ApplicationException : Exception
{
    public int StatusCode { get; }
    
    public IReadOnlyDictionary<string, string[]>? Errors { get; }

    protected ApplicationException(string message, int statusCode = StatusCodes.Status400BadRequest, IReadOnlyDictionary<string, string[]>? errors = null) : base(message)
    {
        StatusCode = statusCode;
        Errors = errors;
    }

    protected ApplicationException(string message, Exception innerException, int statusCode = StatusCodes.Status400BadRequest, IReadOnlyDictionary<string, string[]>? errors = null): base(message, innerException)
    {
        StatusCode = statusCode;
        Errors = errors;
    }
}