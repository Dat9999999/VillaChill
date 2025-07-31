using Microsoft.AspNetCore.Http;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IAmenityRepository : IRepository<Amenity>
{
    public void Update(Amenity amenity);
    Task BulkInsert(IFormFile file);
}