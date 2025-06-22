using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IAmenityRepository : IRepository<Amenity>
{
    public void Update(Amenity amenity);
}