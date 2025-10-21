using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Orders;
using Ofima.Orders.Application.Interfaces;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Enums;
using Ofima.Orders.Domain.Exceptions;
using Ofima.Orders.Domain.Interfaces;

namespace Ofima.Orders.Application.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<PagedResult<OrderDto>>> GetOrdersAsync(OrderFiltersDto filters, CancellationToken cancellationToken = default)
    {
        try
        {
            filters.ValidateAndCorrect();

            var orders = await _unitOfWork.Orders.GetOrdersWithFiltersAsync(
                filters.Status, filters.FromDate, filters.ToDate, filters.CustomerId,
                filters.Page, filters.PageSize, cancellationToken);

            var totalCount = await _unitOfWork.Orders.CountOrdersWithFiltersAsync(
                filters.Status, filters.FromDate, filters.ToDate, filters.CustomerId, cancellationToken);

            var orderDtos = orders.Select(MapToDto);
            var pagedResult = new PagedResult<OrderDto>(orderDtos, filters.Page, filters.PageSize, totalCount);

            return ApiResponse<PagedResult<OrderDto>>.SuccessResult(pagedResult);
        }
        catch (Exception ex)
        {
            return ApiResponse<PagedResult<OrderDto>>.ErrorResult($"Error retrieving orders: {ex.Message}");
        }
    }

    public async Task<ApiResponse<OrderDto?>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var order = await _unitOfWork.Orders.GetWithDetailsAsync(id, cancellationToken);
            if (order == null)
            {
                return ApiResponse<OrderDto?>.ErrorResult("Order not found");
            }

            return ApiResponse<OrderDto?>.SuccessResult(MapToDto(order));
        }
        catch (Exception ex)
        {
            return ApiResponse<OrderDto?>.ErrorResult($"Error retrieving order: {ex.Message}");
        }
    }

    public async Task<ApiResponse<OrderDto>> CreateAsync(OrderCreateDto dto, int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            // Validar cliente
            var customer = await _unitOfWork.Customers.GetByIdAsync(dto.CustomerId, cancellationToken);
            if (customer == null || !customer.IsActive)
            {
                return ApiResponse<OrderDto>.ErrorResult("Invalid or inactive customer");
            }

            // Validar productos y precios
            var productIds = dto.Lines.Select(l => l.ProductId).Distinct().ToList();
            var products = await _unitOfWork.Products.FindAsync(p => productIds.Contains(p.Id), cancellationToken);
            
            if (products.Count() != productIds.Count)
            {
                return ApiResponse<OrderDto>.ErrorResult("One or more products not found");
            }

            // Crear orden
            var orderNumber = await _unitOfWork.Orders.GenerateOrderNumberAsync(cancellationToken);
            var order = new Order
            {
                Number = orderNumber,
                CustomerId = dto.CustomerId,
                Status = OrderStatus.New,
                CreatedBy = userId,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            // Crear líneas
            foreach (var lineDto in dto.Lines)
            {
                var product = products.First(p => p.Id == lineDto.ProductId);
                var orderLine = new OrderLine
                {
                    ProductId = lineDto.ProductId,
                    Qty = lineDto.Qty,
                    UnitPrice = lineDto.UnitPrice,
                    CreatedAt = DateTime.UtcNow
                };
                order.OrderLines.Add(orderLine);
            }

            // Calcular totales
            order.CalculateTotals();

            await _unitOfWork.Orders.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Registrar auditoría
            await _unitOfWork.AuditLogs.LogAsync(userId, "Order", order.Id, "CREATE", 
                new { orderId = order.Id, customerId = order.CustomerId, total = order.Total }, 
                cancellationToken: cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var createdOrder = await _unitOfWork.Orders.GetWithDetailsAsync(order.Id, cancellationToken);
            return ApiResponse<OrderDto>.SuccessResult(MapToDto(createdOrder!), "Order created successfully");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<OrderDto>.ErrorResult($"Error creating order: {ex.Message}");
        }
    }

    public async Task<ApiResponse<OrderDto>> UpdateAsync(int id, OrderUpdateDto dto, int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var order = await _unitOfWork.Orders.GetWithDetailsAsync(id, cancellationToken);
            if (order == null)
            {
                return ApiResponse<OrderDto>.ErrorResult("Order not found");
            }

            if (!order.CanBeEdited)
            {
                return ApiResponse<OrderDto>.ErrorResult($"Order {order.Number} cannot be edited in current state: {order.Status}");
            }

            // Limpiar líneas existentes
            order.OrderLines.Clear();

            // Validar productos
            var productIds = dto.Lines.Select(l => l.ProductId).Distinct().ToList();
            var products = await _unitOfWork.Products.FindAsync(p => productIds.Contains(p.Id), cancellationToken);
            
            if (products.Count() != productIds.Count)
            {
                return ApiResponse<OrderDto>.ErrorResult("One or more products not found");
            }

            // Agregar nuevas líneas
            foreach (var lineDto in dto.Lines)
            {
                var orderLine = new OrderLine
                {
                    OrderId = order.Id,
                    ProductId = lineDto.ProductId,
                    Qty = lineDto.Qty,
                    UnitPrice = lineDto.UnitPrice,
                    CreatedAt = DateTime.UtcNow
                };
                order.OrderLines.Add(orderLine);
            }

            order.Notes = dto.Notes;
            order.CalculateTotals();

            await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Registrar auditoría
            await _unitOfWork.AuditLogs.LogAsync(userId, "Order", order.Id, "UPDATE", 
                new { orderId = order.Id, total = order.Total }, 
                cancellationToken: cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var updatedOrder = await _unitOfWork.Orders.GetWithDetailsAsync(order.Id, cancellationToken);
            return ApiResponse<OrderDto>.SuccessResult(MapToDto(updatedOrder!), "Order updated successfully");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<OrderDto>.ErrorResult($"Error updating order: {ex.Message}");
        }
    }

    public async Task<ApiResponse<OrderDto>> ConfirmAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var order = await _unitOfWork.Orders.GetWithDetailsAsync(id, cancellationToken);
            if (order == null)
            {
                return ApiResponse<OrderDto>.ErrorResult("Order not found");
            }

            if (!order.CanBeConfirmed)
            {
                return ApiResponse<OrderDto>.ErrorResult($"Order {order.Number} cannot be confirmed in current state: {order.Status}");
            }

            // Validar y reservar stock
            foreach (var line in order.OrderLines)
            {
                var stock = await _unitOfWork.Stocks.GetByProductIdAsync(line.ProductId, cancellationToken);
                if (stock == null || !stock.CanReserve(line.Qty))
                {
                    throw new InsufficientStockException(line.ProductId, line.Qty, stock?.Available ?? 0);
                }

                await _unitOfWork.Stocks.ReserveStockAsync(line.ProductId, line.Qty, cancellationToken);
            }

            order.Confirm(userId);
            await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Registrar auditoría
            await _unitOfWork.AuditLogs.LogAsync(userId, "Order", order.Id, "CONFIRM", 
                new { orderId = order.Id, status = "Confirmed", stockReserved = true }, 
                cancellationToken: cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var confirmedOrder = await _unitOfWork.Orders.GetWithDetailsAsync(order.Id, cancellationToken);
            return ApiResponse<OrderDto>.SuccessResult(MapToDto(confirmedOrder!), "Order confirmed successfully");
        }
        catch (InsufficientStockException ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<OrderDto>.ErrorResult($"Insufficient stock: {ex.Message}");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<OrderDto>.ErrorResult($"Error confirming order: {ex.Message}");
        }
    }

    public async Task<ApiResponse<OrderDto>> CancelAsync(int id, int userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var order = await _unitOfWork.Orders.GetWithDetailsAsync(id, cancellationToken);
            if (order == null)
            {
                return ApiResponse<OrderDto>.ErrorResult("Order not found");
            }

            if (!order.CanBeCanceled)
            {
                return ApiResponse<OrderDto>.ErrorResult($"Order {order.Number} cannot be canceled in current state: {order.Status}");
            }

            // Liberar stock reservado
            foreach (var line in order.OrderLines)
            {
                await _unitOfWork.Stocks.ReleaseStockAsync(line.ProductId, line.Qty, cancellationToken);
            }

            order.Cancel(userId);
            await _unitOfWork.Orders.UpdateAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Registrar auditoría
            await _unitOfWork.AuditLogs.LogAsync(userId, "Order", order.Id, "CANCEL", 
                new { orderId = order.Id, status = "Canceled", stockReleased = true }, 
                cancellationToken: cancellationToken);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var canceledOrder = await _unitOfWork.Orders.GetWithDetailsAsync(order.Id, cancellationToken);
            return ApiResponse<OrderDto>.SuccessResult(MapToDto(canceledOrder!), "Order canceled successfully");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return ApiResponse<OrderDto>.ErrorResult($"Error canceling order: {ex.Message}");
        }
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            Number = order.Number,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.Name ?? "",
            Status = order.Status,
            SubTotal = order.SubTotal,
            TaxAmount = order.TaxAmount,
            Total = order.Total,
            CreatedAt = order.CreatedAt,
            ConfirmedAt = order.ConfirmedAt,
            CanceledAt = order.CanceledAt,
            CreatedBy = order.CreatedBy,
            CreatedByUsername = order.CreatedByUser?.Username ?? "",
            Notes = order.Notes,
            OrderLines = order.OrderLines.Select(line => new OrderLineDto
            {
                Id = line.Id,
                ProductId = line.ProductId,
                ProductSku = line.Product?.Sku ?? "",
                ProductName = line.Product?.Name ?? "",
                Qty = line.Qty,
                UnitPrice = line.UnitPrice,
                LineTotal = line.LineTotal
            }).ToList()
        };
    }
}
