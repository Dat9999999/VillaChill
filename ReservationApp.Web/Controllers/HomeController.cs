using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Models;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class HomeController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IBookingService _bookingService;
    public HomeController(IVillaNumberService villaNumberService, IVillaService villaService,
        IBookingService bookingService)
    {
        _villaNumberService = villaNumberService;
        _villaService = villaService;
        _bookingService = bookingService;
    }

    public IActionResult Index()
    {
        HomeVM home = new ()
        {
            VillaList = _villaService.GetAll(includeProperties: "Amenities"),
            CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            Nights = 1
        };
        return View(home);
    }
    [HttpPost]
    public IActionResult Index(HomeVM homevm)
    {
        homevm.VillaList = _villaService.GetAll("Amenities");
        return View(homevm);
    }
    [HttpPost]
    public IActionResult CheckAvailability(int nights, DateOnly checkInDate)
    {
        
        var villaList = _villaService.GetAll("Amenities");
        var villasBooked = _bookingService.GetAll(u => u.Status == SD.StatusApproved 
                                                            || u.Status == SD.StatusCheckedIn).ToList();
        var villaNumbers = _villaNumberService.GetAll().ToList();
        foreach (var villa in villaList)
        {
            int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumbers ,checkInDate, nights, villasBooked, out _);
            villa.IsAvaliable = roomAvailable > 0;
        }
        HomeVM home = new HomeVM()
        {
            VillaList = villaList,
            CheckInDate = checkInDate,
            Nights = nights
        };
        return PartialView("_VillasList",home);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}