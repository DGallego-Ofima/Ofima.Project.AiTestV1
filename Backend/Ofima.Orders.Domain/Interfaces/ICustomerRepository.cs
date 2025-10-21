using Ofima.Orders.Domain.Entities;

namespace Ofima.Orders.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetByTaxIdAsync(string taxId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByTaxIdAsync(string taxId, int? excludeId = null, CancellationToken cancellationToken = default);
}
