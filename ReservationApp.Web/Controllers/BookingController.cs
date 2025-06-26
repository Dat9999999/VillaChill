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
    private readonly IVnPayService _vnPayService;
    public BookingController(IUnitOfWork unitOfWork, IVnPayService vnPayService)
    {
        _unitOfWork = unitOfWork;
        _vnPayService = vnPayService;
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

        return RedirectToAction(nameof(CreatePaymentUrlVnpay), new { bookingId = booking.Id });

    }
    public IActionResult CreatePaymentUrlVnpay(int bookingId)
    {
        var booking = _unitOfWork.Bookings.Get(x => x.Id == bookingId, "Villa");

        var model = new PaymentInformationModel
        {
            Amount = booking.TotalCost * SD.UsdDiffVND,
            Name = booking.Name,
            OrderDescription = $"Okay",
            OrderType = "other",
        };

        var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
        return Redirect(url);
    }

    [HttpGet]
    public IActionResult PaymentCallbackVnpay()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);
        
        return Json(response);
    }
    public IActionResult BookingConfirmation(int bookingid)
    {
        return View(bookingid);
    }

}