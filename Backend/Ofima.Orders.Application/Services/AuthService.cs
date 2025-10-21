using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ofima.Orders.Application.DTOs.Auth;
using Ofima.Orders.Application.DTOs.Common;
using Ofima.Orders.Application.Interfaces;
using Ofima.Orders.Domain.Interfaces;

namespace Ofima.Orders.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(request.Username, cancellationToken);
            if (user == null || !user.IsActive)
            {
                return ApiResponse<LoginResponse>.ErrorResult("Invalid username or password");
            }

            var passwordHash = HashPassword(request.Password);
            if (!user.PasswordHash.SequenceEqual(passwordHash))
            {
                return ApiResponse<LoginResponse>.ErrorResult("Invalid username or password");
            }

            var token = await GenerateTokenAsync(user.Id, user.Username, user.Role);
            var expiresAt = DateTime.UtcNow.AddHours(8); // Token válido por 8 horas

            await _unitOfWork.Users.UpdateLastLoginAsync(user.Id, DateTime.UtcNow, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role,
                    IsActive = user.IsActive
                }
            };

            return ApiResponse<LoginResponse>.SuccessResult(response, "Login successful");
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginResponse>.ErrorResult($"Login failed: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-here-must-be-at-least-32-characters");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return ApiResponse<bool>.SuccessResult(true);
        }
        catch
        {
            return ApiResponse<bool>.SuccessResult(false);
        }
    }

    public async Task<string> GenerateTokenAsync(int userId, string username, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "your-secret-key-here-must-be-at-least-32-characters");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static byte[] HashPassword(string password)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(password));
    }
}
