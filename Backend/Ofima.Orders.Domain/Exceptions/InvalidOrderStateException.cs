using Ofima.Orders.Domain.Enums;

namespace Ofima.Orders.Domain.Exceptions;

public class InvalidOrderStateException : DomainException
{
    public string OrderNumber { get; }
    public OrderStatus CurrentStatus { get; }
    public string Operation { get; }

    public InvalidOrderStateException(string orderNumber, OrderStatus currentStatus, string operation)
        : base($"Cannot {operation} order {orderNumber} in current state: {currentStatus}")
    {
        OrderNumber = orderNumber;
        CurrentStatus = currentStatus;
        Operation = operation;
    }
}
