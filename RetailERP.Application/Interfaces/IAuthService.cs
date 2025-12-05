using RetailERP.Application.DTOs.Auth;

namespace RetailERP.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterUserDto registerUser);
    Task<AuthResultDto> LoginAsync(LoginRequestDto loginRequest);
}
