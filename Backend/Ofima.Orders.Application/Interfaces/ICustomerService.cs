using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Customers;

namespace Ofima.Orders.Application.Interfaces;

public interface ICustomerService
{
    Task<ApiResponse<IEnumerable<CustomerDto>>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task<ApiResponse<CustomerDto?>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponse<CustomerDto>> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default);
    Task<ApiResponse<CustomerDto>> UpdateAsync(int id, CustomerUpdateDto dto, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
