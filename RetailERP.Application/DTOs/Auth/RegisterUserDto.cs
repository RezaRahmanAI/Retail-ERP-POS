using System.ComponentModel.DataAnnotations;
using RetailERP.Domain.Enums;

namespace RetailERP.Application.DTOs.Auth;

public class RegisterUserDto
{
    [Required]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }
}
