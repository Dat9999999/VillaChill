using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class RatingController : Controller
{
    private readonly IRatingService _ratingService;

    public RatingController(IRatingService ratingService)
    {
        _ratingService = ratingService;       
    }
    [HttpPost]
    public IActionResult Create([FromBody] RatingRequestDTO ratingRequest)
    {
        return Ok(new { message = "Rating received successfully!" });
    }
}