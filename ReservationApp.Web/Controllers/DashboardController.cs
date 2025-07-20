using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IOwnerBalanceService _ownerBalanceService;
    public DashboardController( IDashboardService dashboardService, IOwnerBalanceService ownerBalanceService)
    {
        _dashboardService = dashboardService;
        _ownerBalanceService = ownerBalanceService;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetTotalBookingRadialChartData()
    {
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
        return Json(new {currentBalance =_ownerBalanceService.GetBalance(ownerEmail)});
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
}