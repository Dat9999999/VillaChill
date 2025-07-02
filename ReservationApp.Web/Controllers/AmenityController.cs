using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

[Authorize(Roles = "Admin")]
public class AmenityController : Controller
{
    private  readonly IUnitOfWork _unitOfWork;
    private readonly IVillaService _villaService;
    private readonly IAmenityService _amenityService;
    public AmenityController(IUnitOfWork unitOfWork, IVillaService villaService, IAmenityService amenityService)
    {
        _unitOfWork = unitOfWork;
        _villaService = villaService;
        _amenityService = amenityService;
    }
    // GET
    public IActionResult Index()
    {
        var Amenities = _amenityService.GetAll("Villa");
        return View(Amenities);
    }

    public IActionResult Create()
    {
        AmenitiesVM obj = new AmenitiesVM()
        {
            villas = _villaService.GetAll().Select(u => new SelectListItem
            {
            Text = u.Name,
            Value = u.Id.ToString()
        })
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Create(AmenitiesVM obj)
    {
        if (ModelState.IsValid && _amenityService.Add(obj.Amenity, out string errorMessage))
        {
            
            TempData["Success"] = "Amenity is created successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Amenity created failure";
        obj.villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );
        return View(obj);
    }
    public IActionResult Update(int Id)
    {
        AmenitiesVM obj = new AmenitiesVM()
        {
            villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Amenity = _amenityService.GetById(Id)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Update(AmenitiesVM obj)
    {
        if (ModelState.IsValid)
        {
           _amenityService.Update(obj.Amenity);
            TempData["Success"] = "Amenity is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Amenity updated failure";
        obj.villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );
        return View(obj);
    }
    public IActionResult Delete(int Id)
    {
        AmenitiesVM obj = new AmenitiesVM()
        {
            villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Amenity = _unitOfWork.Amenities.Get(x => x.Id == Id)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Delete(AmenitiesVM obj)
    {
        if (_amenityService.Delete(obj.Amenity, out string errorMessage))
        {
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = errorMessage;
        return RedirectToAction("Error", "Home");
    }
}