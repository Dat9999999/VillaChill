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
    
    public IActionResult FinalizeBooking(int VillaId, DateOnly CheckIn, int Nights)
    {
        Booking booking = new Booking()
        {
            VillaId = VillaId,
            CheckInDate = CheckIn,
            Nights = Nights,
            Villa = _unitOfWork.Villas.Get(x => x.Id == VillaId, "Amenities")
        };
        
        return View(booking);
    }
}