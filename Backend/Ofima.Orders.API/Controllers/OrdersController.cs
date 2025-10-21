using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Orders;
using Ofima.Orders.Application.Interfaces;

namespace Ofima.Orders.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Get orders with filtering and pagination
    /// </summary>
    /// <param name="status">Filter by order status</param>
    /// <param name="from">Filter from date</param>
    /// <param name="to">Filter to date</param>
    /// <param name="customerId">Filter by customer ID</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 100)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of orders</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<OrderDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<PagedResult<OrderDto>>>> GetOrders(
        [FromQuery] byte? status = null,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] int? customerId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var filters = new OrderFiltersDto
        {
            Status = status.HasValue ? (Domain.Enums.OrderStatus)status.Value : null,
            FromDate = from,
            ToDate = to,
            CustomerId = customerId,
            Page = page,
            PageSize = pageSize
        };

        var result = await _orderService.GetOrdersAsync(filters, cancellationToken);
        
        // Add X-Total-Count header for client pagination
        if (result.Success && result.Data != null)
        {
            Response.Headers.Add("X-Total-Count", result.Data.Total.ToString());
        }

        return Ok(result);
    }

    /// <summary>
    /// Get order by ID with full details
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Order details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrder(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _orderService.GetByIdAsync(id, cancellationToken);
        
        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new order
    /// </summary>
    /// <param name="dto">Order creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created order</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder(
        [FromBody] OrderCreateDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<OrderDto>.ErrorResult(errors));
        }

        var userId = GetCurrentUserId();
        if (userId == 0)
        {
            return Unauthorized(ApiResponse<OrderDto>.ErrorResult("Invalid user context"));
        }

        var result = await _orderService.CreateAsync(dto, userId, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetOrder), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Update an existing order (only if status is New)
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="dto">Order update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated order</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> UpdateOrder(
        int id, 
        [FromBody] OrderUpdateDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<OrderDto>.ErrorResult(errors));
        }

        var userId = GetCurrentUserId();
        if (userId == 0)
        {
            return Unauthorized(ApiResponse<OrderDto>.ErrorResult("Invalid user context"));
        }

        var result = await _orderService.UpdateAsync(id, dto, userId, cancellationToken);
        
        if (!result.Success)
        {
            if (result.Message?.Contains("cannot be edited") == true)
            {
                return Conflict(result);
            }
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Confirm an order (reserve stock and change status to Confirmed)
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Confirmed order</returns>
    [HttpPost("{id}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> ConfirmOrder(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
        {
            return Unauthorized(ApiResponse<OrderDto>.ErrorResult("Invalid user context"));
        }

        var result = await _orderService.ConfirmAsync(id, userId, cancellationToken);
        
        if (!result.Success)
        {
            if (result.Message?.Contains("stock") == true || 
                result.Message?.Contains("cannot be confirmed") == true)
            {
                return Conflict(result);
            }
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Cancel an order (release reserved stock and change status to Canceled)
    /// </summary>
    /// <param name="id">Order ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Canceled order</returns>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<OrderDto>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CancelOrder(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
        {
            return Unauthorized(ApiResponse<OrderDto>.ErrorResult("Invalid user context"));
        }

        var result = await _orderService.CancelAsync(id, userId, cancellationToken);
        
        if (!result.Success)
        {
            if (result.Message?.Contains("cannot be canceled") == true)
            {
                return Conflict(result);
            }
            return NotFound(result);
        }

        return Ok(result);
    }
}
