using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
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
        return View();
    }
}