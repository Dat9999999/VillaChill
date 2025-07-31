using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Models;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class HomeController : Controller
{
    private readonly IVillaNumberService _villaNumberService;
    private readonly IVillaService _villaService;
    private readonly IBookingService _bookingService;
    private readonly ICacheService _cacheService;
    public HomeController(IVillaNumberService villaNumberService, IVillaService villaService,
        IBookingService bookingService, ICacheService cacheService)
    {
        _villaNumberService = villaNumberService;
        _villaService = villaService;
        _bookingService = bookingService;
        _cacheService = cacheService;      
    }

    public IActionResult Index()
    {
        HomeVM home = new ()
        {
            CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            Nights = 1,
        };
        return View(home);
    }

    [HttpGet]
    public IActionResult LoadMoreVillas(int page = 1, int pageSize = 6,
        string? city = null, DateOnly checkInDate = default, int nights = 1)
    {
        var villaList = _bookingService.CheckAvailability(nights, checkInDate,city, page, pageSize);
        
        HomeVM home = new HomeVM()
        {
            VillaList = villaList,
            CheckInDate = checkInDate,
            Nights = nights,
            HasSearched = true,
            City = city,
        };
        return PartialView("_VillaCard",home);
    }
    [HttpPost]
    public IActionResult Index(HomeVM homevm)
    {
        homevm.VillaList = _villaService.GetAll(null,"Amenities");
        return View(homevm);
    }
    [HttpPost]
    public IActionResult CheckAvailability(int nights, DateOnly checkInDate, string city)
    {
        var villaList = _bookingService.CheckAvailability(nights, checkInDate,city, 1, 6);
        
        HomeVM home = new HomeVM()
        {
            VillaList = villaList,
            CheckInDate = checkInDate,
            Nights = nights,
            HasSearched = true,
            City = city,
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