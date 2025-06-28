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

    public IActionResult Index()
    {
        return View();   
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

    public IActionResult BookingDetails(int bookingId)
    {
        var bookingFromDB = _unitOfWork.Bookings.Get(x => x.Id == bookingId, "User,Villa");
        bookingFromDB.Villa.Amenities = _unitOfWork.Amenities.GetAll(x => x.VillaId == bookingFromDB.VillaId);

        if (bookingFromDB.Status == SD.StatusApproved && bookingFromDB.VillaNumber == 0)
        {
            var availableVillaNumber = AssignVillaNumber(bookingFromDB.VillaId);
            bookingFromDB.VillaNumbers = _unitOfWork.VillaNumbers.GetAll(u => u.Villa.Id == bookingFromDB.VillaId
                && availableVillaNumber.Contains(u.Villa_Number)).ToList();
        } 
        return View(bookingFromDB);   
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
            Name = booking.Email,
            OrderDescription = $"{booking.Id}",
            OrderType = "booking",
        };

        var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
        return Redirect(url);
    }

    [HttpGet]
    public IActionResult PaymentCallbackVnpay()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);
        if (response.VnPayResponseCode == "00")
        {
            var value   = response.OrderDescription.Split(" ");
            var email = value[0];
            // get bookingID
            var bookingId = int.Parse(value[1]);
            
            // cost in VND currency
            var totalCost = value[2];
            
            //update booking after payment successfully
            _unitOfWork.Bookings.UpdateStatus(bookingId, SD.StatusApproved, 0);
            _unitOfWork.Bookings.UpdatePaymentId(bookingId, response.PaymentId);
            _unitOfWork.Save();
            
            return View(nameof(BookingConfirmation), bookingId);
        }
        return RedirectToAction("Error","Home");
    }
    public IActionResult BookingConfirmation(int bookingid )
    {
        return View(bookingid);
    }

    private List<int> AssignVillaNumber(int villaId)
    {
        List<int> availableVillaNumber = new List<int>();
        var villaNumbers = _unitOfWork.VillaNumbers.GetAll(u => u.Villa.Id == villaId);
        var checkedInVillaNumber = _unitOfWork.Bookings.GetAll(u => u.Villa.Id == villaId 
        && u.Status == SD.StatusCheckedIn).Select(u => u.VillaNumber).ToList();
        foreach (var villaNumber in villaNumbers)
        {
            if (!checkedInVillaNumber.Contains(villaNumber.Villa_Number))
            {
                availableVillaNumber.Add(villaNumber.Villa_Number);
            }
        }

        return availableVillaNumber;

    }
    [HttpPost]
    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult CheckIn(Booking booking)
    {
        _unitOfWork.Bookings.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);
        _unitOfWork.Save();
        TempData["Success"] = "Booking is checked in successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }

    #region  API  Call

    [HttpGet]
    public IActionResult GetAll(string status)
    {
            IEnumerable<Booking> objBookings;
            // string userId = "";
            // if (string.IsNullOrEmpty(status))
            // {
            //     status = "";
            // }
            //
            // if (!User.IsInRole(SD.Role_Admin))
            // {
            //     var claimsIdentity = (ClaimsIdentity)User.Identity;
            //     userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            // }
            //
            // objBookings = _unitOfWork.Bookings.GetAll(x => x.Status == status && x.UserId == userId);
            //         
            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objBookings = _unitOfWork.Bookings.GetAll(x=> x.UserId == userId);
            }
            else objBookings = _unitOfWork.Bookings.GetAll();

            if (!string.IsNullOrEmpty(status))
            {
                objBookings = objBookings.Where(x => x.Status.ToLower() == status.ToLower());           
            }
            return Json(new { data = objBookings });
    }
    

    #endregion

}