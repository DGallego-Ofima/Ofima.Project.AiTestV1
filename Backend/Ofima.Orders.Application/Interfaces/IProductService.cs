using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Products;

namespace Ofima.Orders.Application.Interfaces;

public interface IProductService
{
    Task<ApiResponse<IEnumerable<ProductDto>>> GetActiveProductsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsWithStockAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<ProductDto?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<ProductDto?>> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
}
