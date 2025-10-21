using Ofima.Orders.Domain.Entities;

namespace Ofima.Orders.Domain.Interfaces;

public interface IAuditLogRepository : IRepository<AuditLog>
{
    Task<IEnumerable<AuditLog>> GetByEntityAsync(string entity, int entityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetByUserAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> GetRecentActivityAsync(int count = 50, CancellationToken cancellationToken = default);
    Task LogAsync(int userId, string entity, int entityId, string action, object? payload = null, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default);
}
