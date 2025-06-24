using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

[Authorize]
public class VillaNumberController : Controller
{
    private  readonly IUnitOfWork _unitOfWork;
    public VillaNumberController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GET
    public IActionResult Index()
    {
        var villaNumbers = _unitOfWork.VillaNumbers.GetAll(null, "Villa");
        return View(villaNumbers);
    }

    public IActionResult Create()
    {
        VillaNumbersVM obj = new VillaNumbersVM()
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
    public IActionResult Create(VillaNumbersVM obj)
    {
        var isExists = _unitOfWork.VillaNumbers.Any(u => u.Villa_Number == obj.villaNumber.Villa_Number);
        if (ModelState.IsValid && !isExists)
        {
            _unitOfWork.VillaNumbers.Add(obj.villaNumber);
            _unitOfWork.Save();
            TempData["Success"] = "Villa number is created successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Villa number is exists";
        obj.villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );
        return View(obj);
    }
    public IActionResult Update(int VillaNumberId)
    {
        VillaNumbersVM obj = new VillaNumbersVM()
        {
            villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            villaNumber = _unitOfWork.VillaNumbers.Get(x => x.Villa_Number == VillaNumberId)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Update(VillaNumbersVM obj)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.VillaNumbers.Update(obj.villaNumber);
            _unitOfWork.Save();
            TempData["Success"] = "Villa number is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Villa number updated failure";
        obj.villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );
        return View(obj);
    }
    public IActionResult Delete(int VillaNumberId)
    {
        VillaNumbersVM obj = new VillaNumbersVM()
        {
            villas = _unitOfWork.Villas.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            villaNumber = _unitOfWork.VillaNumbers.Get(x => x.Villa_Number == VillaNumberId)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Delete(VillaNumbersVM obj)
    {
        var objToDelete = _unitOfWork.VillaNumbers.Get(x => x.Villa_Number== obj.villaNumber.Villa_Number);
        if (objToDelete is not null)
        {
            _unitOfWork.VillaNumbers.Delete(objToDelete);
            _unitOfWork.Save();
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return RedirectToAction("Error", "Home");
    }
}