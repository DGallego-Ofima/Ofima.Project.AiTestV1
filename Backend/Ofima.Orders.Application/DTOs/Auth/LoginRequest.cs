using System.ComponentModel.DataAnnotations;

namespace Ofima.Orders.Application.DTOs.Auth;

public class LoginRequest
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 80 characters")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
    public string Password { get; set; } = string.Empty;
}
