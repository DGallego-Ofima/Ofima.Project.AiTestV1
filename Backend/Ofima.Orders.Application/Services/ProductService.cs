using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Products;
using Ofima.Orders.Application.Interfaces;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Interfaces;

namespace Ofima.Orders.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetActiveProductsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var products = await _unitOfWork.Products.GetActiveProductsAsync(cancellationToken);
            var productDtos = products.Select(MapToDto);
            return ApiResponse<IEnumerable<ProductDto>>.SuccessResult(productDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<ProductDto>>.ErrorResult($"Error retrieving products: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsWithStockAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var products = await _unitOfWork.Products.GetProductsWithStockAsync(cancellationToken);
            var productDtos = products.Select(MapToDto);
            return ApiResponse<IEnumerable<ProductDto>>.SuccessResult(productDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<ProductDto>>.ErrorResult($"Error retrieving products with stock: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ProductDto?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
            if (product == null)
            {
                return ApiResponse<ProductDto?>.ErrorResult("Product not found");
            }

            return ApiResponse<ProductDto?>.SuccessResult(MapToDto(product));
        }
        catch (Exception ex)
        {
            return ApiResponse<ProductDto?>.ErrorResult($"Error retrieving product: {ex.Message}");
        }
    }

    public async Task<ApiResponse<ProductDto?>> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _unitOfWork.Products.GetBySkuAsync(sku, cancellationToken);
            if (product == null)
            {
                return ApiResponse<ProductDto?>.ErrorResult("Product not found");
            }

            return ApiResponse<ProductDto?>.SuccessResult(MapToDto(product));
        }
        catch (Exception ex)
        {
            return ApiResponse<ProductDto?>.ErrorResult($"Error retrieving product: {ex.Message}");
        }
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            Stock = product.Stock != null ? new StockDto
            {
                ProductId = product.Stock.ProductId,
                OnHand = product.Stock.OnHand,
                Reserved = product.Stock.Reserved,
                Available = product.Stock.Available,
                LastUpdatedAt = product.Stock.LastUpdatedAt
            } : null
        };
    }
}
