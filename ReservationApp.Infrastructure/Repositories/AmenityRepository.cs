using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class AmenityRepository: Repository<Amenity>, IAmenityRepository
{
    private  readonly ApplicationDbContext _context;
    public AmenityRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Amenity amenity)
    {
        _context.Amenities.Update(amenity);
    }

    public async Task BulkInsert(IFormFile file)
    {
        var amenities = new List<Amenity>();
        var existingAmenities = _context.Amenities
                .Select(x => new {x.VillaId, x.Name})
                .ToHashSet();
        
        
        
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            ExcelPackage.License.SetNonCommercialOrganization("My University Club");
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var name = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                        var villaIdStr = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                        var description = worksheet.Cells[row, 3].Value?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(villaIdStr))
                            continue; // skip error row

                        int villaId = int.Parse(villaIdStr);
                        {
                            amenities.Add(new Amenity()
                            {
                                Name = name,
                                VillaId = villaId,
                                Description = description ?? ""
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error:
                        Console.WriteLine($"Get error at row: {row}: {ex.Message}");
                    }
                }
            }
        }

        if (amenities.Count > 0)
        {
            var validAmenities = amenities
                .Where(a => !existingAmenities.Contains(new { a.VillaId, a.Name }))
                .ToList();
            _context.BulkInsert(validAmenities);
            await _context.SaveChangesAsync();
        }
    }
}