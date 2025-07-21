using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Hubs;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IOwnerBalanceService _ownerBalanceService;
    private readonly IHubContext<DashBoardHub> _hubContext;
    
    
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
        IOwnerBalanceService ownerBalanceService, IHubContext<DashBoardHub> hubContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _ownerBalanceService = ownerBalanceService;
        _hubContext = hubContext;
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

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVm.Email, loginVm.Password, loginVm.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(loginVm.Email);
                if (await _userManager.IsInRoleAsync(user, SD.Role_Admin) ||
                    await _userManager.IsInRoleAsync(user, SD.Role_Owner))
                {
                    return RedirectToAction("Index", "Dashboard");   
                }
                if (string.IsNullOrEmpty(loginVm.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                return Redirect(loginVm.ReturnUrl);
            }
            ModelState.AddModelError("", "Invalid login attempt.");
        }
        return View(loginVm);
    }
    public IActionResult Register(string returnUrl = null)
    {
        returnUrl??= Url.Content("~/");
        
        RegisterVM registerVm = new()
        {
            Roles = _roleManager.Roles.Select(u => new SelectListItem()
            {
                Text = u.Name,
                Value = u.Name
            })
        };
        registerVm.ReturnUrl = returnUrl;
        return View(registerVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM registerVm)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser user = new()
            {
                Name = registerVm.Name,
                Email = registerVm.Email,
                UserName = registerVm.Email,
                NormalizedEmail = registerVm.Email.ToUpper(),
                PhoneNumber = registerVm.PhoneNumber,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now,
            };
            var result = await _userManager.CreateAsync(user, registerVm.Password);
            await _hubContext.Clients.All.SendAsync("UserRegistered", new {user.Name, user.Email, user.Id});
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(registerVm.Role))
                {
                    if (registerVm.Role == SD.Role_Owner)
                    {
                        //create owner balance
                       var res =  _ownerBalanceService.Create(user.Email);   
                    }
                    await _userManager.AddToRoleAsync(user, registerVm.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                if (string.IsNullOrEmpty(registerVm.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }

                return Redirect(registerVm.ReturnUrl);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        registerVm.Roles = _roleManager.Roles.Select(u => new SelectListItem()
        {
            Text = u.Name,
            Value = u.Name
        });
        return View(registerVm);
    }
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AccessDenied()
    {
        return View();   
    }
    
}