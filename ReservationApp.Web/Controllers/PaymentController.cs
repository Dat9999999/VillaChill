using Microsoft.AspNetCore.Mvc;

namespace ReservationApp.Controllers;

public class PaymentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}