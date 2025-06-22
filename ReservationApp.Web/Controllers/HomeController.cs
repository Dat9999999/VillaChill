using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
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
        return View(home);
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