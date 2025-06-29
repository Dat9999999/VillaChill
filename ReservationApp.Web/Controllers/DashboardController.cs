using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class DashboardController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
    private DateTime previousStartMonthDate = new DateTime(DateTime.Now.Year, previousMonth, 1);
    private DateTime currentStartMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    
    public DashboardController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetTotalBookingRadialChartData()
    {
        var totalBookings = _unitOfWork.Bookings.GetAll(u => u.Status != SD.StatusCancelled
                                                             && u.Status != SD.StatusPending);
        var totalBookingsByCurrMonth = totalBookings.Count(x => x.BookingDate >= currentStartMonthDate && x.BookingDate <= DateTime.Now);
        var totalBookingsByPrevMonth = totalBookings.Count(x => x.BookingDate >= previousStartMonthDate && x.BookingDate < currentStartMonthDate);
        RadialBarChartVM chart = new RadialBarChartVM();
        int ratio = 100;
        if (totalBookingsByPrevMonth != 0)
        {
            ratio = Convert.ToInt32(totalBookingsByCurrMonth - totalBookingsByPrevMonth / totalBookingsByPrevMonth * 100);
        }
        chart.CountInCurrentMonth = totalBookingsByCurrMonth;
        chart.TotalCount = totalBookings.Count();
        chart.IsIncrease = totalBookingsByCurrMonth > totalBookingsByPrevMonth;
        chart.series = new []{ratio};
        return Json(chart);
    }
}