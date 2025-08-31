namespace NatoursApi.Domain.Exceptions;

public class BadRequestException(string message) : ApplicationException(message);