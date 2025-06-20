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
    [HttpPost]
    public IActionResult Create(VillaNumbersVM obj)
    {
        var isExists = _context.VillaNumbers.Any(u => u.Villa_Number == obj.villaNumber.Villa_Number);
        if (ModelState.IsValid && !isExists)
        {
            _context.VillaNumbers.Add(obj.villaNumber);
            _context.SaveChanges();
            TempData["Success"] = "Villa number is created successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Villa number is exists";
        obj.villas = _context.Villas.ToList().Select(u => new SelectListItem
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
            villas = _context.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            villaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == VillaNumberId)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Update(VillaNumbersVM obj)
    {
        if (ModelState.IsValid)
        {
            _context.VillaNumbers.Update(obj.villaNumber);
            _context.SaveChanges();
            TempData["Success"] = "Villa number is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        TempData["Error"] = "Villa number updated failure";
        obj.villas = _context.Villas.ToList().Select(u => new SelectListItem
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
            villas = _context.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            villaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == VillaNumberId)
        };
        return View(obj);
    }
    [HttpPost]
    public IActionResult Delete(VillaNumbersVM obj)
    {
        var objToDelete = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number== obj.villaNumber.Villa_Number);
        if (objToDelete is not null)
        {
            _context.VillaNumbers.Remove(objToDelete);
            _context.SaveChanges();
            TempData["Success"] = "Villa deleted successfully";
            return RedirectToAction(nameof(Index));
        } 
        TempData["Error"] = "Villa not found";
        return RedirectToAction("Error", "Home");
    }
}