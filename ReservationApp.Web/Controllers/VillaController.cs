using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Controllers;

public class VillaController : Controller
{
    private  readonly IVillaRepository _villaRepository;
    public VillaController(IVillaRepository villaRepository)
    {
        _villaRepository = villaRepository;
    }
    // GET
    public IActionResult Index()
    {
        var villas = _villaRepository.GetAll();
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
        _villaRepository.Add(obj);
        _villaRepository.Save();
        TempData["Success"] = "Villa created successfully";
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int villaId)
    {
        var villa = _villaRepository.Get(x => x.Id == villaId);
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
            _villaRepository.Update(obj);
            _villaRepository.Save();
            TempData["Success"] = "Villa updated successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return View(obj);
    }
    public IActionResult Delete(int villaId)
    {
        var villa = _villaRepository.Get(x => x.Id == villaId);
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
        var objToDelete = _villaRepository.Get(x => x.Id == obj.Id);
        if (objToDelete is not null)
        {
            _villaRepository.Delete(objToDelete);
            _villaRepository.Save();
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return RedirectToAction("Error", "Home");
    }
}