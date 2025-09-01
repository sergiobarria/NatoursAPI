using Microsoft.AspNetCore.Http;

namespace NatoursApi.Domain.Exceptions;

public class UnauthorizedException(string message = "Unauthorized")
    : ApplicationException(message, StatusCodes.Status401Unauthorized);