using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class CommissionRateController : Controller
{
    private readonly IComissionService _comissionService;

    public  CommissionRateController(IComissionService comissionService)
    {
        _comissionService  = comissionService;
    }
    // GET
    public IActionResult Index()
    {
        var commissionRates = _comissionService.GetAll();
        return View(commissionRates);
    }

    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult Create()
    {
        return View();
    }
    [Authorize(Roles = SD.Role_Admin)]
    [HttpPost]
    public IActionResult Create(CommissionRateRequestDTO obj)
    {
        if (ModelState.IsValid)
        {
            var commissionRate = _comissionService.Add(obj);
            TempData["Success"] = $"Commission rate created successfully. Commission rate is {commissionRate.Rate}%";
            return RedirectToAction(nameof(Index));
        }
        return View(obj);
    }
    [Authorize(Roles = SD.Role_Admin)]
    public IActionResult Update(int Id)
    {
        var commissionRate = _comissionService.Get(x=> x.Id == Id);
        return View(commissionRate);
    }
    [Authorize(Roles = SD.Role_Admin)]
    [HttpPost]
    public IActionResult Update(CommissionRate obj)
    {

        if (ModelState.IsValid)
        {
            _comissionService.Update(obj);
            TempData["Success"] = $"{obj.Name} updated, update commission rate successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Update failure";
        return View(obj);
    }
    public IActionResult Delete(int Id)
    {
        var commissionRate = _comissionService.Get(x=> x.Id == Id);
        return View(commissionRate);
    }
    [Authorize(Roles = SD.Role_Admin)]
    [HttpPost]
    public IActionResult Delete(CommissionRate obj)
    {
        if (ModelState.IsValid)
        {
            _comissionService.Delete(obj);
            TempData["Success"] = $"{obj.Name} updated, update commission rate successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Delete failure";
        return View(obj);
    }
    #region API
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetByName(string name)
    {
        var ratePlatformFee = _comissionService.Get(x => x.Name.ToLower() == name.ToLower());
        return Json(ratePlatformFee?.Rate ?? 0);
    }
    #endregion
}