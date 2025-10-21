using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Orders;

namespace Ofima.Orders.Application.Interfaces;

public interface IOrderService
{
    Task<ApiResponse<PagedResult<OrderDto>>> GetOrdersAsync(OrderFiltersDto filters, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderDto?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderDto>> CreateAsync(OrderCreateDto dto, int userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderDto>> UpdateAsync(int id, OrderUpdateDto dto, int userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderDto>> ConfirmAsync(int id, int userId, CancellationToken cancellationToken = default);
    Task<ApiResponse<OrderDto>> CancelAsync(int id, int userId, CancellationToken cancellationToken = default);
}
