using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly IVnPayService _vnPayService;
    private readonly IExporter _exporter;
    private readonly IBookingService _bookingService;
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IAmenityService _amenityService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IOwnerBalanceService _ownerBalanceService;
    public BookingController(IVnPayService vnPayService, IExporter exporter,
        IBookingService bookingService, IVillaNumberService villaNumberService, IVillaService villaService,
        IAmenityService amenityService,
        UserManager<ApplicationUser> userManager, IEmailService emailService,
        IOwnerBalanceService ownerBalanceService
        )
    {
        _vnPayService = vnPayService;
        _exporter = exporter;
        _bookingService = bookingService;
        _villaNumberService = villaNumberService;
        _villaService = villaService;
        _amenityService = amenityService;
        _userManager = userManager;
        _emailService = emailService;
        _ownerBalanceService = ownerBalanceService;

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
        
        ApplicationUser user = _userManager.FindByIdAsync(userId).Result;
        var villa = _villaService.GetById(VillaId, "Amenities");
        var roomAvailable = AssignVillaNumber(VillaId);
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
            VillaNumbers = roomAvailable
            
        };
        return View(booking);
    }

    public IActionResult BookingDetails(int bookingId)
    {
        var bookingFromDB = _bookingService.GetById(x => x.Id == bookingId, "User,Villa");
        bookingFromDB.Villa.Amenities = _amenityService.GetAll(x => x.VillaId == bookingFromDB.VillaId);

        if (bookingFromDB.Status == SD.StatusApproved || bookingFromDB.Status == SD.StatusCompleted || bookingFromDB.Status == SD.StatusCheckedIn)
        {
            var availableVillaNumber = AssignVillaNumber(bookingFromDB.VillaId);
            bookingFromDB.VillaNumbers = availableVillaNumber;
        } 
        return View(bookingFromDB);   
    }
    [HttpPost]
    public IActionResult FinalizeBooking(Booking booking)
    {
        var villa = _villaService.GetById(booking.VillaId);
        booking.TotalCost = villa.Price * booking.Nights;
        
        booking.Status = SD.StatusPending;
        booking.BookingDate = DateTime.Now;
        
        _bookingService.Add(booking);

        return RedirectToAction(nameof(CreatePaymentUrlVnpay), new { bookingId = booking.Id });

    }
    public IActionResult CreatePaymentUrlVnpay(int bookingId)
    {
        var booking = _bookingService.GetById(x => x.Id == bookingId, "Villa");

        var model = new PaymentInformationModel
        {
            Amount = booking.TotalCost,
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
            var customerEmail = value[0];
            // get bookingID
            var bookingId = int.Parse(value[1]);
            var booking = _bookingService.GetById(x => x.Id == bookingId);
            var villa = _villaService.GetById(booking.VillaId);
            
            //update booking after payment successfully
            _bookingService.UpdateStatus(bookingId, SD.StatusApproved, booking.VillaNumber);
            _bookingService.UpdatePaymentId(bookingId, response.PaymentId);
            
            //sending notification that booking successfully through email 
            _emailService.configMailPaySuccess(customerEmail, villa.Name, booking.VillaNumber);
            
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
        var villaNumbers = _villaNumberService.GetAll(u => u.Villa.Id == villaId);
        var checkedInVillaNumber = _bookingService.GetAll(u => u.Villa.Id == villaId 
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
        _bookingService.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);
        TempData["Success"] = "Booking is checked in successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }
    [HttpPost]
    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult CheckOut(Booking booking)
    {
        _bookingService.UpdateStatus(booking.Id, SD.StatusCompleted, booking.VillaNumber);
        if (!booking.IsPaidAtCheckIn)
        {
            // update ownerbalance when customer using payment online method 
            _ownerBalanceService.UpdateBalance(booking.Id);
        }
        TempData["Success"] = "Booking is checked out successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }
    [HttpPost]
    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult Cancel(Booking booking)
    {
        _bookingService.UpdateStatus(booking.Id, SD.StatusCancelled, 0);
        TempData["Success"] = "Booking is Canceled successfully";
        return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
    }

    public IActionResult DownloadInvoice(int bookingId)
    {
        var booking = _bookingService.GetById(x => x.Id == bookingId, "Villa");
        if (booking is null) return NotFound();
        
        //check if user download their invoice 
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (!(User.IsInRole(SD.Role_Admin)) && booking.UserId != userId) return Unauthorized();

        var stream = _exporter.ExportBookingInvoice(booking);
        var fileName = $"Invoice_{booking.Id}.docx";
        return File(stream, 
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 
            fileName);
    }


    #region  API  Call

    [HttpGet]
    public IActionResult GetAll(string status)
    {
            IEnumerable<Booking> objBookings;         
            if (!User.IsInRole(SD.Role_Admin))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objBookings = _bookingService.GetAll(x=> x.UserId == userId);
            }
            else objBookings = _bookingService.GetAll();

            if (!string.IsNullOrEmpty(status))
            {
                objBookings = objBookings.Where(x => x.Status.ToLower() == status.ToLower());           
            }
            return Json(new { data = objBookings });
    }
    

    #endregion

}