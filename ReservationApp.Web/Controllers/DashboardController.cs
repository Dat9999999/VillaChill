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
        var result = new ColumnChartDTO();

        // Ví dụ mẫu – bạn có thể tính từ DB theo `range`
        switch (range)
        {
            case "1m":
                result.categories = new List<string> { "Week 1", "Week 2", "Week 3", "Week 4" };
                result.data = new List<decimal> { 120000, 150000, 170000, 900000 };
                break;
            case "3m":
                result.categories = new List<string> { "Jan", "Feb", "Mar" };
                result.data = new List<decimal> { 400000, 500000, 600000 };
                break;
            case "6m":
                result.categories = new List<string> { "Week 1", "Week 2", "Week 3", "Week 4" };
                result.data = new List<decimal> { 120000, 150000, 170000, 200000 };
                break;
            case "12m":
                result.categories = new List<string> { "Jan", "Feb", "Mar" };
                result.data = new List<decimal> { 400000, 500000, 600000 };
                break;
        }
        return Json(result);
    }

}