using Microsoft.EntityFrameworkCore;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Enums;
using Ofima.Orders.Domain.Interfaces;
using Ofima.Orders.Infrastructure.Data;

namespace Ofima.Orders.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(OrdersDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(o => o.Number == orderNumber, cancellationToken);
    }

    public async Task<Order?> GetWithDetailsAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.Customer)
            .Include(o => o.CreatedByUser)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOrdersWithFiltersAsync(
        OrderStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? customerId = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet
            .Include(o => o.Customer)
            .Include(o => o.CreatedByUser)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(o => o.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(o => o.CreatedAt <= toDate.Value);

        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId.Value);

        return await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountOrdersWithFiltersAsync(
        OrderStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? customerId = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (fromDate.HasValue)
            query = query.Where(o => o.CreatedAt >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(o => o.CreatedAt <= toDate.Value);

        if (customerId.HasValue)
            query = query.Where(o => o.CustomerId == customerId.Value);

        return await query.CountAsync(cancellationToken);
    }

    public async Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow;
        var prefix = $"PED-{today:yyyy}-";
        
        var lastOrderToday = await _dbSet
            .Where(o => o.Number.StartsWith(prefix))
            .OrderByDescending(o => o.Number)
            .FirstOrDefaultAsync(cancellationToken);

        if (lastOrderToday == null)
        {
            return $"{prefix}001";
        }

        var lastNumberPart = lastOrderToday.Number.Substring(prefix.Length);
        if (int.TryParse(lastNumberPart, out var lastNumber))
        {
            return $"{prefix}{(lastNumber + 1):D3}";
        }

        return $"{prefix}001";
    }

    public async Task<bool> ExistsByNumberAsync(string orderNumber, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(o => o.Number == orderNumber);
        
        if (excludeId.HasValue)
            query = query.Where(o => o.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}
