using Microsoft.AspNetCore.Http;

namespace NatoursApi.Domain.Exceptions;

public class ForbiddenException(string message = "Forbidden.")
    : ApplicationException(message, StatusCodes.Status403Forbidden);