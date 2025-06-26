using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Controllers;

public class BookingController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public BookingController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public IActionResult FinalizeBooking(int VillaId, string checkInDate, int Nights)
    {
        if (!DateOnly.TryParseExact(checkInDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            ModelState.AddModelError("CheckInDate", "Invalid date format. Please use dd/MM/yyyy.");
            return BadRequest("Invalid date format.");
        }

        var villa = _unitOfWork.Villas.Get(x => x.Id == VillaId, "Amenities");

        Booking booking = new Booking()
        {
            VillaId = VillaId,
            CheckInDate = parsedDate,
            Nights = Nights,
            CheckOutDate = parsedDate.AddDays(Nights),
            Villa = villa,
            TotalCost = villa.Price * Nights
        };

        return View(booking);
    }
    [HttpPost]
    public IActionResult FinalizeBooking(Booking booking)
    {
        
        return View(booking);
    }
}