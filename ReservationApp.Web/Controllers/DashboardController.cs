using Microsoft.AspNetCore.Mvc;

namespace ReservationApp.Controllers;

public class DashboardController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}