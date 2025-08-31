using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NatoursApi.Domain.Entities;
using NatoursApi.Domain.Enums;

namespace NatoursApi.Data.Interceptors;

public class AuditLogsInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        UpdateAuditEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateAuditEntities(DbContext? context)
    {
        if (context is null) return;

        var entriesToAudit = context.ChangeTracker.Entries().Where(entry =>
            entry.Entity is not AuditLog && !entry.State.Equals(EntityState.Detached) &&
            !entry.State.Equals(EntityState.Unchanged)).ToList();

        var auditLogsToAdd = new List<AuditLog>();

        foreach (var entry in entriesToAudit)
        {
            if (entry.Entity is AuditLog || entry.State.Equals(EntityState.Detached) ||
                entry.State.Equals(EntityState.Unchanged)) continue;

            var auditLog = new AuditLog
            {
                Timestamp = DateTime.UtcNow,
                Username = "System",
                EntityName = entry.Entity.GetType().Name,
                EntityId = GetEntityId(entry)
            };

            switch (entry.State)
            {
                case EntityState.Added:
                    auditLog.Action = AuditAction.Created;
                    auditLog.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                    break;

                case EntityState.Modified:
                    // Only log audit if there are changes
                    if (entry.Properties.Any(p => p.IsModified && !Equals(p.OriginalValue, p.CurrentValue)))
                    {
                        auditLog.Action = AuditAction.Modified;
                        auditLog.OldValues = JsonSerializer.Serialize(GetOriginalValues(entry));
                        auditLog.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        auditLog.ChangedColumns =
                            JsonSerializer.Serialize(entry.Properties.Where(p => p.IsModified)
                                .Select(p => p.Metadata.Name));
                    }
                    else
                    {
                        continue;
                    }

                    break;

                case EntityState.Deleted:
                    auditLog.Action = AuditAction.Deleted;
                    auditLog.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    break;
            }

            auditLogsToAdd.Add(auditLog);
        }

        if (auditLogsToAdd.Any()) context.Set<AuditLog>().AddRange(auditLogsToAdd);
    }

    private static Guid GetEntityId(EntityEntry entry)
    {
        var primaryKey = entry.Metadata.FindPrimaryKey();
        if (primaryKey is null) return Guid.Empty;

        var keyValue = entry.Property(primaryKey.Properties.First().Name).CurrentValue;
        if (keyValue is Guid guidValue) return guidValue;

        return Guid.Empty;
    }

    private static Dictionary<string, object?> GetOriginalValues(EntityEntry entry)
    {
        var originalValues = new Dictionary<string, object?>();
        foreach (var property in entry.Properties)
            if (property.IsModified)
                originalValues[property.Metadata.Name] = property.OriginalValue;

        return originalValues;
    }
}