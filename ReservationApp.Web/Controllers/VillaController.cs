using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
namespace ReservationApp.Controllers;

[Authorize]
public class VillaController : Controller
{
    
    private readonly IVillaService _villaService;
    public VillaController(IVillaService villaService)
    {
        _villaService = villaService;
    }
    // GET
    public IActionResult Index()
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var role = claimIdentity.FindFirst(ClaimTypes.Role).Value;
        var email = claimIdentity.FindFirst(ClaimTypes.Name).Value;
        if (role == SD.Role_Owner)
        {
            return View(_villaService.GetAll(x=> x.OwnerEmail == email));
        }
        return View(_villaService.GetAll());
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Villa obj)
    {
        var claimIdentity = (ClaimsIdentity)User.Identity;
        var email = claimIdentity.FindFirst(ClaimTypes.Email).Value;
        obj.OwnerEmail = email;
        if (!_villaService.Add(obj, out string errorMessage))
        {
            if(errorMessage == "Name and Description cannot be the same")
            {
                ModelState.AddModelError("Description", "Name and Description cannot be the same");
            }
            return View(obj);
        }
        
        TempData["Success"] = "Villa created successfully";
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int villaId)
    {
        var villa = _villaService.GetById(villaId);
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
            _villaService.Update(obj);
            TempData["Success"] = "Villa updated successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return View(obj);
    }
    public IActionResult Delete(int villaId)
    {
        var villa = _villaService.GetById(villaId);
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
        if (_villaService.Delete(obj, out string errorMessage))
        {
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = errorMessage;
        return RedirectToAction("Error", "Home");
    }
}