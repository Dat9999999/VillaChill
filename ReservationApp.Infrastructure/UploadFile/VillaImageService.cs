using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ReservationApp.Application.Common.Interfaces;

namespace ReservationApp.Infrastructure.UploadFile;

public class VillaImageService : IVillaImageService
{
    private readonly IWebHostEnvironment _env;
    
    public VillaImageService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public string SaveImage(IFormFile file)
    {
        string fileName = Guid.NewGuid() + Path.GetFileName(file.FileName);
        string folderPath = Path.Combine(_env.WebRootPath, "images/VillaImage");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fullPath = Path.Combine(folderPath, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return $"/images/VillaImage/{fileName}";
    }
}