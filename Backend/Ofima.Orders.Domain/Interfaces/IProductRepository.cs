using Ofima.Orders.Domain.Entities;

namespace Ofima.Orders.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySkuAsync(string sku, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetProductsWithStockAsync(CancellationToken cancellationToken = default);
}
