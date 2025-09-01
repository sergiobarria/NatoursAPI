using Microsoft.AspNetCore.Http;

namespace NatoursApi.Domain.Exceptions;

public class ValidationException : ApplicationException
{
    public ValidationException(IReadOnlyDictionary<string, string[]>? errors) : base(
        "One or more validation failures have occurred.", StatusCodes.Status422UnprocessableEntity)
    {
    }

    public ValidationException(string message) : base(message, StatusCodes.Status422UnprocessableEntity)
    {
    }

    public ValidationException(string propertyName, string message) : base("One or more validation failures occurred.",
        StatusCodes.Status422UnprocessableEntity,
        new Dictionary<string, string[]> { { propertyName, [message] } })
    {
    }
}