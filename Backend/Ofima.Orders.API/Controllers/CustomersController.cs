using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.DTOs.Customers;
using Ofima.Orders.Application.Interfaces;

namespace Ofima.Orders.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Get all active customers
    /// </summary>
    /// <param name="active">Filter by active status (default: true)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active customers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerDto>>>> GetCustomers(
        [FromQuery] bool active = true,
        CancellationToken cancellationToken = default)
    {
        if (active)
        {
            var result = await _customerService.GetActiveCustomersAsync(cancellationToken);
            return Ok(result);
        }

        // For now, we only support active customers
        var activeResult = await _customerService.GetActiveCustomersAsync(cancellationToken);
        return Ok(activeResult);
    }

    /// <summary>
    /// Get customer by ID
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> GetCustomer(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _customerService.GetByIdAsync(id, cancellationToken);
        
        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new customer
    /// </summary>
    /// <param name="dto">Customer creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created customer</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> CreateCustomer(
        [FromBody] CustomerCreateDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<CustomerDto>.ErrorResult(errors));
        }

        var result = await _customerService.CreateAsync(dto, cancellationToken);
        
        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetCustomer), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Update an existing customer
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="dto">Customer update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated customer</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<CustomerDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<CustomerDto>>> UpdateCustomer(
        int id, 
        [FromBody] CustomerUpdateDto dto, 
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage);
            return BadRequest(ApiResponse<CustomerDto>.ErrorResult(errors));
        }

        var result = await _customerService.UpdateAsync(id, dto, cancellationToken);
        
        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Delete a customer (soft delete)
    /// </summary>
    /// <param name="id">Customer ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(
        int id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _customerService.DeleteAsync(id, cancellationToken);
        
        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
