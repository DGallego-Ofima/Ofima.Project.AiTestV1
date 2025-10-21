using Ofima.Orders.Application.DTOs.Auth;
using Ofima.Orders.Application.DTOs.Common;

namespace Ofima.Orders.Application.Interfaces;

public interface IAuthService
{
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<bool>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<string> GenerateTokenAsync(int userId, string username, string role);
}
