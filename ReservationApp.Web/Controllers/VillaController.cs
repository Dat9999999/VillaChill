using Microsoft.AspNetCore.Mvc;
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
}