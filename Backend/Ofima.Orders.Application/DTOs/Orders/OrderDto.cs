using Ofima.Orders.Domain.Enums;

namespace Ofima.Orders.Application.DTOs.Orders;

public class OrderDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public string StatusName => Status.ToString();
    public decimal SubTotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public int CreatedBy { get; set; }
    public string CreatedByUsername { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<OrderLineDto> OrderLines { get; set; } = new();
}

public class OrderCreateDto
{
    public int CustomerId { get; set; }
    public string? Notes { get; set; }
    public List<OrderLineCreateDto> Lines { get; set; } = new();
}

public class OrderUpdateDto
{
    public string? Notes { get; set; }
    public List<OrderLineCreateDto> Lines { get; set; } = new();
}

public class OrderLineDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductSku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class OrderLineCreateDto
{
    public int ProductId { get; set; }
    public int Qty { get; set; }
    public decimal UnitPrice { get; set; }
}
