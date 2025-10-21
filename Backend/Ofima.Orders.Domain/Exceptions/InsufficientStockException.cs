namespace Ofima.Orders.Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    public int ProductId { get; }
    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }

    public InsufficientStockException(int productId, int requestedQuantity, int availableQuantity)
        : base($"Insufficient stock for product {productId}. Requested: {requestedQuantity}, Available: {availableQuantity}")
    {
        ProductId = productId;
        RequestedQuantity = requestedQuantity;
        AvailableQuantity = availableQuantity;
    }
}
