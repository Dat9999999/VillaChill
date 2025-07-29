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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);       
        }
        _ratingService.Add(ratingRequest);       
        return Ok(new { message = "Rating received successfully!" });
    }
    [HttpGet]
    public IActionResult GetRatingsByVillaId([FromQuery]int villaId)
    {
        var ratings = _ratingService.GetAll(u => u.VillaId == villaId);
        return Ok(ratings);       
    }
}