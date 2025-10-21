using Microsoft.EntityFrameworkCore;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Interfaces;
using Ofima.Orders.Infrastructure.Data;

namespace Ofima.Orders.Infrastructure.Repositories;

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(OrdersDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AuditLog>> GetByEntityAsync(string entity, int entityId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.User)
            .Where(a => a.Entity == entity && a.EntityId == entityId)
            .OrderByDescending(a => a.At)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditLog>> GetByUserAsync(int userId, DateTime? fromDate = null, DateTime? toDate = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(a => a.User)
            .Where(a => a.UserId == userId);

        if (fromDate.HasValue)
            query = query.Where(a => a.At >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(a => a.At <= toDate.Value);

        return await query
            .OrderByDescending(a => a.At)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<AuditLog>> GetRecentActivityAsync(int count = 50, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.User)
            .OrderByDescending(a => a.At)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task LogAsync(int userId, string entity, int entityId, string action, object? payload = null, string? ipAddress = null, string? userAgent = null, CancellationToken cancellationToken = default)
    {
        var auditLog = AuditLog.Create(userId, entity, entityId, action, payload, ipAddress, userAgent);
        await _dbSet.AddAsync(auditLog, cancellationToken);
    }
}
