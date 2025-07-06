using Microsoft.AspNetCore.Mvc;

namespace ReservationApp.Controllers;

public class RatingController : Controller
{
    // GET
    public IActionResult CreateRating()
    {
        return View();
    }
}