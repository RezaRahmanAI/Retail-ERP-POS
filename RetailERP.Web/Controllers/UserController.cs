using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailERP.Domain.Constants;
using RetailERP.Domain.Entities;

namespace RetailERP.Web.Controllers;

//[Authorize(Roles = RoleConstants.AdminAndManager)]
public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // List Users
    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users;
        return View(await users.ToListAsync());
    }
}
