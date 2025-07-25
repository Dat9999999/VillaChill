using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Controllers;

[Authorize]
public class OwnerSettlementController : Controller
{
    private readonly IOwnerSettlementService _ownerSettlementService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVnPayService _vnPayService;
    public OwnerSettlementController(IOwnerSettlementService ownerSettlementService, UserManager<ApplicationUser> userManager,
    IVnPayService vnPayService)
    {
        _ownerSettlementService = ownerSettlementService;
        _userManager = userManager;
        _vnPayService = vnPayService;
    }
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSettlements()
    {
        return Json(_ownerSettlementService.GetAll(_userManager.GetUserId(User),User.IsInRole(SD.Role_Admin)));
    }
    [HttpPost]
    public IActionResult BulkPay([FromBody] List<int> bookingIds)
    {
        TempData["BookingIds"] = JsonConvert.SerializeObject(bookingIds); // cần Newtonsoft.Json
        return RedirectToAction(nameof(CreatePaymentUrlVnpay));
    }

    public IActionResult CreatePaymentUrlVnpay()
    {
        var json = TempData["BookingIds"] as string;
        var bookingIds = JsonConvert.DeserializeObject<List<int>>(json);

        var model = _ownerSettlementService.MarkAsPaidBulk(bookingIds);
        var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
        return Content(url);
    }


    [HttpGet]
    public IActionResult PaymentCallbackVnpay()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);
        if (response.VnPayResponseCode == "00")
        {
            var listIdFromResponse = response.OrderDescription.Split(" ")[1].Split(",");

            var bookingIdList = listIdFromResponse
                .Where(id => int.TryParse(id, out _)) // lọc những cái convert được
                .Select(int.Parse)
                .ToList();

            _ownerSettlementService.UpdatePaymentStatus(bookingIdList, SD.StatusPayment_Paid);
            
            return View(nameof(Index));
        }
        return RedirectToAction("Error","Home");
    }

    [HttpPost]
    public IActionResult RestrictOverdueOwners([FromBody] List<string> ownerIds)
    {
        _ownerSettlementService.RestrictOverdue(ownerIds);
        return Ok(new { success = true, count = ownerIds.Count });
    }
    

}