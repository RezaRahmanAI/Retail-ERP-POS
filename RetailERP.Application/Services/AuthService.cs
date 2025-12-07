using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RetailERP.Application.DTOs.Auth;
using RetailERP.Application.Interfaces;
using RetailERP.Domain.Entities;
using RetailERP.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RetailERP.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterUserDto registerUser)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerUser.Email);
        if (existingUser is not null)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = new[] { "User already exists with the provided email address." }
            };
        }

        var newUser = new ApplicationUser
        {
            Email = registerUser.Email,
            UserName = registerUser.Email,
            FullName = registerUser.FullName
        };

        var createdUser = await _userManager.CreateAsync(newUser, registerUser.Password);

        if (!createdUser.Succeeded)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = createdUser.Errors.Select(error => error.Description)
            };
        }

        var roleName = registerUser.Role.ToString();

        await EnsureRoleExistsAsync(roleName);

        var roleResult = await _userManager.AddToRoleAsync(newUser, roleName);
        if (!roleResult.Succeeded)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = roleResult.Errors.Select(error => error.Description)
            };
        }

        return GenerateToken(newUser, roleName);
    }

    public async Task<AuthResultDto> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user is null)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = new[] { "Invalid credentials." }
            };
        }

        var passwordValid = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
        if (!passwordValid.Succeeded)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = new[] { "Invalid credentials." }
            };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var roleName = roles.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(roleName))
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = new[] { "User is not assigned to a role." }
            };
        }

        return GenerateToken(user, roleName);
    }

    private AuthResultDto GenerateToken(ApplicationUser user, string roleName)
    {
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.FullName ?? user.UserName ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, roleName)
        };

        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
        var signingKey = new SymmetricSecurityKey(key);
        var expiresAt = DateTime.UtcNow.AddHours(5);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: expiresAt,
            claims: authClaims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new AuthResultDto
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt
        };
    }

    private async Task EnsureRoleExistsAsync(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName)) return;

        await _roleManager.CreateAsync(new IdentityRole(roleName));
    }
}
