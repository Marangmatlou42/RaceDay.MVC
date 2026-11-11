using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
using System.Security.Claims;

namespace RaceDay.MVC.Controllers;

public class AccountController : Controller
{
    private readonly ApiClient _api;
    public AccountController(ApiClient api) => _api = api;

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string role)
    {
        var response = await _api.PostAsync("api/auth/login", new { email, password, role });

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Email, email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

        return role == "Organiser" ? RedirectToAction("Dashboard", "Organiser") : RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string role, string firstName, string lastName, string companyName, string companyPhone, DateTime? dateOfBirth)
    {
        
        var response = await _api.PostAsync("api/auth/register", new
        {
            email,
            password,
            role,
            firstName,
            lastName,
            companyName,
            companyPhone,
            dateOfBirth
        });

        if (!response.IsSuccessStatusCode)
        {
            
            var errorJson = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"API Error: {errorJson}";
            return View();
        }

       
        return await Login(email, password, role);
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
