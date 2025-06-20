using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;
using ReservationApp.ViewModels;

namespace ReservationApp.Controllers;

public class VillaNumberController : Controller
{
    private  readonly ApplicationDbContext _context;
    public VillaNumberController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        var villaNumbers = _context.VillaNumbers.Include(u => u.Villa).ToList();
        return View(villaNumbers);
    }

    public IActionResult Create()
    {
        VillaNumbersVM obj = new VillaNumbersVM()
        {
            villas = _context.Villas.ToList().Select(u => new SelectListItem
            {
            Text = u.Name,
            Value = u.Id.ToString()
        })
        };
        return View(obj);
    }
    // [HttpPost]
    // public IActionResult Create(VillaNumber obj)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         TempData["Error"] = "Villa number is not created";
    //         return View(obj);
    //     }
    //     _context.VillaNumbers.Add(obj);
    //     _context.SaveChanges();
    //     TempData["Success"] = "Villa number is created successfully";
    //     return RedirectToAction("Index");
    // }
    public IActionResult Update(int villaId)
    {
        var villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
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
            _context.Villas.Update(obj);
            _context.SaveChanges();
            TempData["Success"] = "Villa updated successfully";
            return RedirectToAction("Index");
        } 
        TempData["Error"] = "Villa not found";
        return View(obj);
    }
    public IActionResult Delete(int villaId)
    {
        var villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
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
        var objToDelete = _context.Villas.FirstOrDefault(x => x.Id == obj.Id);
        if (objToDelete is not null)
        {
            _context.Villas.Remove(objToDelete);
            _context.SaveChanges();
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction("Index");
        } 
        TempData["Error"] = "Villa not found";
        return RedirectToAction("Error", "Home");
    }
}