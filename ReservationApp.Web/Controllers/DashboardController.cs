using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

[Authorize(Roles = $"{SD.Role_Owner},{SD.Role_Admin}")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IVillaService _villaService;
    public DashboardController( IDashboardService dashboardService,
        IVillaService villaService)
    {
        _dashboardService = dashboardService;
        _villaService = villaService;
    }
    // GET
    public IActionResult Index()
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var role = claimIdentity.FindFirst(ClaimTypes.Role).Value;
        if (role == SD.Role_Owner)
        {
            var ownerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var villas = _villaService.GetAll(x => x.OwnerEmail == ownerEmail);

            ViewBag.Villas = villas;
        }
        return View();
    }

    public async Task<IActionResult> GetTotalBookingRadialChartData()
    {
        if (!User.IsInRole(SD.Role_Admin))
        {
            var userEmail = User.FindFirst(ClaimTypes.Name).Value;
            return Json(_dashboardService.GetTotalBookingRadialChartData(userEmail));
        }
        return Json(_dashboardService.GetTotalBookingRadialChartData());
    }
    public async Task<IActionResult> GetUserRegisteredRadialChartData()
    {
        return Json(_dashboardService.GetUserRegisteredRadialChartData());
    }
    public async Task<IActionResult> GetBookingPieChartData()
    {
        return Json(_dashboardService.GetBookingPieChartData());
    }
    public async Task<IActionResult> GetRevenueRadialChartData()
    {
        return Json(_dashboardService.GetRevenueRadialChartData());;
    }

    public async Task<IActionResult> getCustomerAndBookingLineChart()
    {
        
        return Json(_dashboardService.getCustomerAndBookingLineChart());
    }

    public IActionResult GetCurrentBalanceRadialChartData([FromQuery] string ownerEmail)
    {
        return Json(new {currentBalance =_dashboardService.GetBalance(ownerEmail)});
    }

    [HttpGet]
    public IActionResult GetVillaBookingPieChart([FromQuery] string ownerEmail)
    {
        return Json(_dashboardService.GetVillaBookingPieChart(ownerEmail));
    }

    [HttpGet]
    public IActionResult GetNumberOfVilla([FromQuery] string ownerEmail)
    {
        return Json(new {count = _dashboardService.GetGetNumberOfVilla(ownerEmail)});
    }
    [HttpGet]
    public IActionResult GetRevenueChartData(string range)
    {
       var userEmail = User.FindFirst(ClaimTypes.Name).Value;
        return Json(_dashboardService.GetRevenueChartData(range, userEmail));
    }
    [HttpGet]
    public IActionResult GetSentimentRatio([FromQuery] int? villaId)
    {
        var ownerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Json(_dashboardService.GetSentimentRatio(ownerId, villaId));

    }


}