using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Controllers;

[Authorize]
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
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        
        var user = _unitOfWork.ApplicationUsers.Get(x => x.Id == userId);
        var villa = _unitOfWork.Villas.Get(x => x.Id == VillaId, "Amenities");
        Booking booking = new Booking()
        {
            VillaId = VillaId,
            CheckInDate = parsedDate,
            Nights = Nights,
            CheckOutDate = parsedDate.AddDays(Nights),
            Villa = villa,
            TotalCost = villa.Price * Nights,
            UserId = userId,
            Name = user.Name,
            Email = user.Email,
            Phone = user.PhoneNumber,
        };
        return View(booking);
    }
    [HttpPost]
    public IActionResult FinalizeBooking(Booking booking)
    {
        var villa = _unitOfWork.Villas.Get(x => x.Id == booking.VillaId);
        booking.TotalCost = villa.Price * booking.Nights;
        
        booking.Status = SD.StatusPending;
        booking.BookingDate = DateTime.Now;
        
        _unitOfWork.Bookings.Add(booking);
        _unitOfWork.Save();
        
        return View(nameof(BookingConfirmation), booking.Id);
    }
    public IActionResult BookingConfirmation(int bookingid)
    {
        return View(bookingid);
    }
}