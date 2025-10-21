using Microsoft.EntityFrameworkCore;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Interfaces;
using Ofima.Orders.Infrastructure.Data;

namespace Ofima.Orders.Infrastructure.Repositories;

public class StockRepository : Repository<Stock>, IStockRepository
{
    public StockRepository(OrdersDbContext context) : base(context)
    {
    }

    public async Task<Stock?> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Product)
            .FirstOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
    }

    public async Task<IEnumerable<Stock>> GetStocksByProductIdsAsync(IEnumerable<int> productIds, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Product)
            .Where(s => productIds.Contains(s.ProductId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasSufficientStockAsync(int productId, int requiredQuantity, CancellationToken cancellationToken = default)
    {
        var stock = await _dbSet.FirstOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
        return stock != null && stock.Available >= requiredQuantity;
    }

    public async Task<IEnumerable<Stock>> GetLowStockProductsAsync(int threshold = 10, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(s => s.Product)
            .Where(s => s.Available <= threshold && s.Product.IsActive)
            .OrderBy(s => s.Available)
            .ToListAsync(cancellationToken);
    }

    public async Task ReserveStockAsync(int productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await _dbSet.FirstOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
        if (stock != null)
        {
            stock.ReserveStock(quantity);
            _dbSet.Update(stock);
        }
    }

    public async Task ReleaseStockAsync(int productId, int quantity, CancellationToken cancellationToken = default)
    {
        var stock = await _dbSet.FirstOrDefaultAsync(s => s.ProductId == productId, cancellationToken);
        if (stock != null)
        {
            stock.ReleaseStock(quantity);
            _dbSet.Update(stock);
        }
    }
}
