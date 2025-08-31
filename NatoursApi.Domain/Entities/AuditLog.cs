using NatoursApi.Domain.Enums;

namespace NatoursApi.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; init; }

    public string EntityName { get; init; } = string.Empty;

    public Guid EntityId { get; init; }

    public AuditAction Action { get; set; }

    public string? Username { get; init; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public string? ChangedColumns { get; set; }

    public DateTime Timestamp { get; init; }
}