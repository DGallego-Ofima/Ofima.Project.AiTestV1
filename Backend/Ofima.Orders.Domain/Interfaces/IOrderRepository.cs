using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Enums;

namespace Ofima.Orders.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<Order?> GetWithDetailsAsync(int orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetOrdersWithFiltersAsync(
        OrderStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? customerId = null,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
    Task<int> CountOrdersWithFiltersAsync(
        OrderStatus? status = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? customerId = null,
        CancellationToken cancellationToken = default);
    Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByNumberAsync(string orderNumber, int? excludeId = null, CancellationToken cancellationToken = default);
}
