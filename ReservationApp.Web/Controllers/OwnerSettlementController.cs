using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Controllers;

[Authorize]
public class OwnerSettlementController : Controller
{
    private readonly IOwnerSettlementService _ownerSettlementService;
    private readonly UserManager<ApplicationUser> _userManager;
    public OwnerSettlementController(IOwnerSettlementService ownerSettlementService, UserManager<ApplicationUser> userManager)
    {
        _ownerSettlementService = ownerSettlementService;
        _userManager = userManager;
    }
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSettlements()
    {
        return Json(_ownerSettlementService.GetAll(_userManager.GetUserId(User),User.IsInRole(SD.Role_Admin)));
    }
}