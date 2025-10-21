using Microsoft.EntityFrameworkCore;
using Ofima.Orders.Domain.Entities;
using Ofima.Orders.Domain.Interfaces;
using Ofima.Orders.Infrastructure.Data;

namespace Ofima.Orders.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(OrdersDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> ValidateCredentialsAsync(string username, byte[] passwordHash, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(u => u.Username == username && 
                                         u.PasswordHash == passwordHash && 
                                         u.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(u => u.IsActive)
                          .OrderBy(u => u.Username)
                          .ToListAsync(cancellationToken);
    }

    public async Task UpdateLastLoginAsync(int userId, DateTime loginTime, CancellationToken cancellationToken = default)
    {
        var user = await _dbSet.FindAsync(new object[] { userId }, cancellationToken);
        if (user != null)
        {
            user.LastLoginAt = loginTime;
            _dbSet.Update(user);
        }
    }
}
