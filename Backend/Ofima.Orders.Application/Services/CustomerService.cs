using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Customers;
using Ofima.Orders.Application.Interfaces;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Interfaces;

namespace Ofima.Orders.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<CustomerDto>>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var customers = await _unitOfWork.Customers.GetActiveCustomersAsync(cancellationToken);
            var customerDtos = customers.Select(MapToDto);
            return ApiResponse<IEnumerable<CustomerDto>>.SuccessResult(customerDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<CustomerDto>>.ErrorResult($"Error retrieving customers: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerDto?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);
            if (customer == null)
            {
                return ApiResponse<CustomerDto?>.ErrorResult("Customer not found");
            }

            return ApiResponse<CustomerDto?>.SuccessResult(MapToDto(customer));
        }
        catch (Exception ex)
        {
            return ApiResponse<CustomerDto?>.ErrorResult($"Error retrieving customer: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerDto>> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validar que el TaxId no exista
            if (await _unitOfWork.Customers.ExistsByTaxIdAsync(dto.TaxId, cancellationToken: cancellationToken))
            {
                return ApiResponse<CustomerDto>.ErrorResult("A customer with this Tax ID already exists");
            }

            var customer = new Customer
            {
                Name = dto.Name,
                TaxId = dto.TaxId,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Customers.AddAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<CustomerDto>.SuccessResult(MapToDto(customer), "Customer created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<CustomerDto>.ErrorResult($"Error creating customer: {ex.Message}");
        }
    }

    public async Task<ApiResponse<CustomerDto>> UpdateAsync(int id, CustomerUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);
            if (customer == null)
            {
                return ApiResponse<CustomerDto>.ErrorResult("Customer not found");
            }

            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;
            customer.Address = dto.Address;
            customer.IsActive = dto.IsActive;
            customer.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Customers.UpdateAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<CustomerDto>.SuccessResult(MapToDto(customer), "Customer updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<CustomerDto>.ErrorResult($"Error updating customer: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id, cancellationToken);
            if (customer == null)
            {
                return ApiResponse<bool>.ErrorResult("Customer not found");
            }

            // Soft delete - marcar como inactivo
            customer.IsActive = false;
            customer.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Customers.UpdateAsync(customer, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<bool>.SuccessResult(true, "Customer deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult($"Error deleting customer: {ex.Message}");
        }
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            TaxId = customer.TaxId,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            IsActive = customer.IsActive,
            CreatedAt = customer.CreatedAt
        };
    }
}
