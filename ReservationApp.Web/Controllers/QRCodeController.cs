using Microsoft.AspNetCore.Mvc;
using ReservationApp.Application.Common.Interfaces;

namespace ReservationApp.Controllers;

public class QRCodeController : Controller
{
    private readonly IQRCoderService _qrCoderService;
    public QRCodeController(IQRCoderService qrCoderService)
    {
        _qrCoderService = qrCoderService;
    }
    [HttpGet("qr")]
    public IActionResult GetQr([FromQuery] string text)
    {
        var qrBytes = _qrCoderService.GenerateQRCode(text ?? "VillaChill");
        return File(qrBytes, "image/png");
    }
}