using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Models;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class HomeController : Controller
{
    private  readonly IUnitOfWork _unitOfWork;
    public HomeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        HomeVM home = new ()
        {
            VillaList = _unitOfWork.Villas.GetAll(null, "Amenities"),
            CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            Nights = 1
        };
        foreach (var item in home.VillaList)
        {
            if (item.Id % 2 == 0)
            {
                item.IsAvaliable = false;
            }
        }
        return View(home);
    }
    [HttpPost]
    public IActionResult Index(HomeVM homevm)
    {
        homevm.VillaList = _unitOfWork.Villas.GetAll(null, "Amenities");
        foreach (var item in homevm.VillaList)
        {
            if (item.Id % 2 == 0)
            {
                item.IsAvaliable = false;
            }
        }
        return View(homevm);
    }
    [HttpPost]
    public IActionResult CheckAvailability(int nights, DateOnly checkInDate)
    {
        
        var villaList = _unitOfWork.Villas.GetAll(null, "Amenities");
        var villasBooked = _unitOfWork.Bookings.GetAll(u => u.Status == SD.StatusApproved 
                                                            || u.Status == SD.StatusCheckedIn).ToList();
        var villaNumbers = _unitOfWork.VillaNumbers.GetAll().ToList();
        
        foreach (var villa in villaList)
        {
            int roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumbers ,checkInDate, nights, villasBooked);
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