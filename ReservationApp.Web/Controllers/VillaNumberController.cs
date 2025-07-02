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

[Authorize]
public class VillaNumberController : Controller
{
    private readonly IVillaService _villaService;
    private readonly IVillaNumberService _villaNumberService;
    public VillaNumberController(IVillaService villaService,
        IVillaNumberService villaNumberService)
    {
        _villaService = villaService;
        _villaNumberService = villaNumberService;
    }
    // GET
    public IActionResult Index()
    {
        var villaNumbers = _villaNumberService.GetAll();
        return View(villaNumbers);
    }

    public IActionResult Create()
    {
        VillaNumbersVM obj = new VillaNumbersVM()
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
    public IActionResult Create(VillaNumbersVM obj)
    {
        if (ModelState.IsValid)
        {
            var res = _villaNumberService.Add(obj.villaNumber, out string errorMessage);
            if (res)
            {
                TempData["Success"] = "Villa number is created successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = errorMessage;
        }
        obj.villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );
        return View(obj);
        
    }
    public IActionResult Update(int VillaNumber)
    {
        VillaNumbersVM obj = new VillaNumbersVM()
        {
            villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            villaNumber = _villaNumberService.GetById(VillaNumber)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Update(VillaNumbersVM obj)
    {
        if (ModelState.IsValid)
        {
            _villaNumberService.Update(obj.villaNumber);
            TempData["Success"] = "Villa number is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Villa number updated failure";
        obj.villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }
        );
        return View(obj);
    }
    public IActionResult Delete(int VillaNumber)
    {
        VillaNumbersVM obj = new VillaNumbersVM()
        {
            villas = _villaService.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            villaNumber = _villaNumberService.GetById(VillaNumber)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Delete(VillaNumbersVM obj)
    {
        if (_villaNumberService.Delete(obj.villaNumber, out string errorMessage))
        {
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = errorMessage;
        return RedirectToAction("Error", "Home");
    }
}