using Microsoft.AspNetCore.Mvc;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Controllers;

public class VillaController : Controller
{
    private  readonly ApplicationDbContext _context;
    public VillaController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET
    public IActionResult Index()
    {
        var villas = _context.Villas.ToList();
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
        _context.Villas.Add(obj);
        _context.SaveChanges();
        TempData["Success"] = "Villa created successfully";
        return RedirectToAction(nameof(Index));
    }
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
            return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return RedirectToAction("Error", "Home");
    }
}