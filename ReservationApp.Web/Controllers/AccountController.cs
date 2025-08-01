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
    private readonly IEmailService _emailService;
    
    
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
        IOwnerBalanceService ownerBalanceService, IHubContext<DashBoardHub> hubContext, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _ownerBalanceService = ownerBalanceService;
        _hubContext = hubContext;
        _emailService = emailService;
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
                user.LastLoginDate = DateOnly.FromDateTime(DateTime.Now);
                await _userManager.UpdateAsync(user);
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
            //mapper
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
            
            //send notification to admin groups
            await _hubContext.Clients.Groups(SD.Role_Admin).SendAsync("UserRegistered", new {user.Name, user.Email, user.Id});
            
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

    public IActionResult ForgotPassword(string returnUrl = null)
    {
        returnUrl??= Url.Content("~/");
        
        ForgotPasswordVM forgotPasswordVm = new()
        {
            Email = ""
        };
        forgotPasswordVm.ReturnUrl = returnUrl;
        return View(forgotPasswordVm);  
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { token, email = model.Email },
                protocol: Request.Scheme);

            // Gửi email tại đây (SMTP hoặc SendGrid)
            _emailService.SendEmail(
                model.Email,
                "Reset Password - VillaChill",
                $"Click here to reset your password: <a href='{callbackUrl}'>link</a>");
        }
        TempData["Success"] = "Please check your email to reset your password.";
        return Redirect(model.ReturnUrl);
    }
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (token == null || email == null)
            return BadRequest("Invalid password reset request.");

        return View(new ResetPasswordVM { Token = token, Email = email });
    }
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return RedirectToAction("ResetPasswordConfirmation");

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (result.Succeeded)
            return RedirectToAction("ResetPasswordConfirmation");

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    public async Task<IActionResult> Index(int? page = 1, int? pageSize = 3)
    {
        var users = _userManager.Users
            .ToList().Skip(pageSize.Value * (page.Value -1)).Take(pageSize.Value);
        var total = _userManager.Users.Count();
        var userVMs = new List<UserVM>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            userVMs.Add(new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name, // nếu có
                AvatarUrl = user.AvatarUrl,
                Role = roles.FirstOrDefault() ?? "None",
                IsLocked = user.LockoutEnd.HasValue,
                CreatedAt = user.CreatedAt,
                LastLoginTime = user.LastLoginDate ?? DateOnly.FromDateTime(DateTime.MinValue),
            });
        }

        UserManagementVM userManagementVm = new UserManagementVM()
        {
            TotalCount = total,
            Users = userVMs,
            CurrentPage = page.Value,
        };

        return View(userManagementVm);  
    }
}