using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class AccountController : Controller
{
    private  readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    
    public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    // GET
    public IActionResult Login(string returnUrl = null)
    {
        returnUrl??= Url.Content("~/");
        LoginVM loginVm = new()
        {
            ReturnUrl = returnUrl
        };
        return View(loginVm);
    }
    public IActionResult Register()
    {
        if (!_roleManager.RoleExistsAsync(SD.Role_Admin).Result)
        {
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
        }
        RegisterVM registerVm = new()
        {
            Roles = _roleManager.Roles.Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Name
            })
        };
        return View(registerVm);
    }
}