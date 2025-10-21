using Microsoft.EntityFrameworkCore;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Interfaces;
using Ofima.Orders.Infrastructure.Data;

namespace Ofima.Orders.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(OrdersDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(p => p.IsActive)
                          .OrderBy(p => p.Name)
                          .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.Sku == sku, cancellationToken);
    }

    public async Task<bool> ExistsBySkuAsync(string sku, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(p => p.Sku == sku);
        
        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetProductsWithStockAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(p => p.Stock)
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }
}
