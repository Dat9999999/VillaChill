using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

[Authorize(Roles = "Admin")]
public class AmenityController : Controller
{
    private  readonly IUnitOfWork _unitOfWork;
    public AmenityController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GET
    public IActionResult Index()
    {
        var Amenities = _unitOfWork.Amenities.GetAll(null, "Villa");
        return View(Amenities);
    }

    public IActionResult Create()
    {
        AmenitiesVM obj = new AmenitiesVM()
        {
            villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
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
        var isExists = _unitOfWork.Amenities.Any(u => u.Id == obj.Amenity.Id);
        if (ModelState.IsValid && !isExists)
        {
            _unitOfWork.Amenities.Add(obj.Amenity);
            _unitOfWork.Save();
            TempData["Success"] = "Amenity is created successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Amenity is exists";
        obj.villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
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
            villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Amenity = _unitOfWork.Amenities.Get(x => x.Id == Id)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Update(AmenitiesVM obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Amenities.Update(obj.Amenity);
            _unitOfWork.Save();
            TempData["Success"] = "Amenity is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Amenity updated failure";
        obj.villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
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
            villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
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
        var objToDelete = _unitOfWork.Amenities.Get(x => x.Id== obj.Amenity.Id);
        if (objToDelete is not null)
        {
            _unitOfWork.Amenities.Delete(objToDelete);
            _unitOfWork.Save();
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return RedirectToAction("Error", "Home");
    }
}