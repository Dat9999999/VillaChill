using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Hubs;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class RatingController : Controller
{
    private readonly IRatingService _ratingService;
    private readonly IHubContext<DashBoardHub> _hubContext;

    public RatingController(IRatingService ratingService, IHubContext<DashBoardHub> hubContext)
    {
        _ratingService = ratingService;
        _hubContext = hubContext;      
    }
    [HttpPost]
    public IActionResult Create([FromBody] RatingRequestDTO ratingRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);       
        }
        _ratingService.Add(ratingRequest); 
        _hubContext.Clients.All.SendAsync("NewRating", ratingRequest);
        return Ok(new { message = "Rating received successfully!" });
    }
    [HttpGet]
    public IActionResult GetRatingsByVillaId([FromQuery]int villaId)
    {
        var ratings = _ratingService.GetAll(u => u.VillaId == villaId);
        return Ok(ratings);       
    }
}