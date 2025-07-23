using Microsoft.AspNetCore.Mvc;

namespace ReservationApp.Controllers;

public class OwnerSettlementController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}