using Microsoft.AspNetCore.Http;

namespace NatoursApi.Domain.Exceptions;

public class ConflictException(string message) : ApplicationException(message, StatusCodes.Status409Conflict);