using Ofima.Orders.Domain.Entities;

namespace Ofima.Orders.Domain.Interfaces;

public interface IStockRepository : IRepository<Stock>
{
    Task<Stock?> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Stock>> GetStocksByProductIdsAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default);
    Task<bool> HasSufficientStockAsync(int productId, int requiredQuantity, CancellationToken cancellationToken = default);
    Task<IEnumerable<Stock>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken = default);
    Task ReserveStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);
    Task ReleaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default);
}
