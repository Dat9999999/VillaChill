using Microsoft.AspNetCore.Http;

namespace ReservationApp.Application.Common.Interfaces;

public interface IVillaImageService
{
    string SaveImage(IFormFile file);
}
