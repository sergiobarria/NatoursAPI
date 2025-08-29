namespace NatoursApi.Domain.Exceptions;

public sealed class NotFoundException(string resource, object? key = null) : Exception(key is null
    ? $"{resource} not found."
    : $"{resource} with key '{key}' not found.")
{
    public string Resource { get; } = resource;

    public object? Key { get; } = key;
}

public sealed class BadRequestException(string message, IDictionary<string, string[]> errors) : Exception(message)
{
    public IDictionary<string, string[]>? Errors { get; } = errors;
}