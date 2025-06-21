using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Controllers;

public class VillaController : Controller
{
    private  readonly IUnitOfWork _unitOfWork;
    public VillaController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GET
    public IActionResult Index()
    {
        var villas = _unitOfWork.Villas.GetAll();
        return View(villas);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Villa obj)
    {
        if (obj.Name == obj.Description)
        {
            ModelState.AddModelError("Description", "Name and Description cannot be the same");
        }
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Villa not created";
            return View(obj);
        }
        _unitOfWork.Villas.Add(obj);
        _unitOfWork.Villas.Save();
        TempData["Success"] = "Villa created successfully";
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int villaId)
    {
        var villa = _unitOfWork.Villas.Get(x => x.Id == villaId);
        if (villa is null)
        {
            return RedirectToAction("Error", "Home");
        }
        return View(villa);
    }
    [HttpPost]
    public IActionResult Update(Villa obj)
    {
        if (ModelState.IsValid && obj.Id > 0)
        {
            _unitOfWork.Villas.Update(obj);
            _unitOfWork.Villas.Save();
            TempData["Success"] = "Villa updated successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return View(obj);
    }
    public IActionResult Delete(int villaId)
    {
        var villa = _unitOfWork.Villas.Get(x => x.Id == villaId);
        if (villa is null)
        {
            TempData["Error"] = "Villa not found";
            return RedirectToAction("Error", "Home");
        }
        return View(villa);
    }
    [HttpPost]
    public IActionResult Delete(Villa obj)
    {
        var objToDelete = _unitOfWork.Villas.Get(x => x.Id == obj.Id);
        if (objToDelete is not null)
        {
            _unitOfWork.Villas.Delete(objToDelete);
            _unitOfWork.Villas.Save();
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return RedirectToAction("Error", "Home");
    }
}