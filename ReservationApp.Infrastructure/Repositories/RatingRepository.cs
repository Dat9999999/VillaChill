using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class RatingRepository : Repository<Rating>, IRatingRepository
{
    private  readonly ApplicationDbContext _context;
    
    public RatingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Rating rating)
    {
        throw new NotImplementedException();
    }
}