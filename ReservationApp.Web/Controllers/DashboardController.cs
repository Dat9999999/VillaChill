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
    public DashboardController( IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }
    // GET
    public IActionResult Index()
    {
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

}