using Ofima.Orders.Domain.Entities;

namespace Ofima.Orders.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ValidateCredentialsAsync(string username, byte[] passwordHash, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    Task UpdateLastLoginAsync(int userId, DateTime loginTime, CancellationToken cancellationToken = default);
}
