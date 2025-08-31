using Microsoft.AspNetCore.Http;

namespace NatoursApi.Domain.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string message) : base(message, StatusCodes.Status404NotFound)
    {
    }

    public NotFoundException(string resourceName, object id) : base($"{resourceName} with id '{id}' not found.",
        StatusCodes.Status404NotFound)
    {
    }
}