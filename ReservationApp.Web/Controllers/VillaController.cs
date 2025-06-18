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
            return View(obj);
        }
        _context.Villas.Add(obj);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    public IActionResult Update(int villaId)
    {
        var villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
        if (villa == null)
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
            return RedirectToAction("Index");
        } 
        return View(obj);
    }
}