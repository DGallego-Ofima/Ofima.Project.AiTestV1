namespace Ofima.Orders.Domain.Enums;

public enum OrderStatus : byte
{
    /// <summary>
    /// Pedido creado, puede ser editado
    /// </summary>
    New = 0,

    /// <summary>
    /// Pedido confirmado, stock reservado
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Pedido anulado, stock liberado
    /// </summary>
    Canceled = 2
}
