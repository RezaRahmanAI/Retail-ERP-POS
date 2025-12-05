using Microsoft.AspNetCore.Mvc;
using RetailERP.Application.DTOs.Auth;
using RetailERP.Application.Interfaces;

namespace RetailERP.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResultDto>> Register([FromBody] RegisterUserDto registerUser)
    {
        var result = await _authService.RegisterAsync(registerUser);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginRequestDto loginRequest)
    {
        var result = await _authService.LoginAsync(loginRequest);

        if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }
}
