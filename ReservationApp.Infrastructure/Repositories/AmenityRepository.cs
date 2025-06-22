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
}