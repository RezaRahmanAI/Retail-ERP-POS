using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RetailERP.Application.DTOs.Auth;
using RetailERP.Web.Settings;
using RetailERP.Web.ViewModels.Auth;

namespace RetailERP.Web.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApiSettings _apiSettings;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiOptions, ILogger<AuthController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _apiSettings = apiOptions.Value;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();
        var endpoint = BuildApiEndpoint("api/Auth/login");

        try
        {
            var response = await client.PostAsJsonAsync(endpoint, new LoginRequestDto
            {
                Email = model.Email,
                Password = model.Password
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResultDto>();

                if (result != null && result.Success)
                {
                    if (!string.IsNullOrWhiteSpace(result.Token))
                    {
                        TempData["AuthToken"] = result.Token;
                    }

                    return RedirectToAction("Dashboard");
                }
            }

            var errorResult = await response.Content.ReadFromJsonAsync<AuthResultDto>();
            if (errorResult?.Errors != null)
            {
                foreach (var error in errorResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Unable to log in. Please check your credentials and try again.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login request failed");
            ModelState.AddModelError(string.Empty, "Login failed due to a server error. Please try again later.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient();
        var endpoint = BuildApiEndpoint("api/Auth/register");

        try
        {
            var response = await client.PostAsJsonAsync(endpoint, new RegisterUserDto
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResultDto>();
                if (result != null && result.Success)
                {
                    TempData["RegistrationMessage"] = "Account created successfully. Please log in.";
                    return RedirectToAction("Login");
                }
            }

            var errorResult = await response.Content.ReadFromJsonAsync<AuthResultDto>();
            if (errorResult?.Errors != null)
            {
                foreach (var error in errorResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Please verify your information and try again.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration request failed");
            ModelState.AddModelError(string.Empty, "Registration failed due to a server error. Please try again later.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }

    private string BuildApiEndpoint(string relativePath)
    {
        var trimmedPath = relativePath.TrimStart('/');
        if (!string.IsNullOrWhiteSpace(_apiSettings.BaseUrl))
        {
            return $"{_apiSettings.BaseUrl.TrimEnd('/')}/{trimmedPath}";
        }

        var requestBase = $"{Request.Scheme}://{Request.Host}";
        return $"{requestBase}/{trimmedPath}";
    }
}
