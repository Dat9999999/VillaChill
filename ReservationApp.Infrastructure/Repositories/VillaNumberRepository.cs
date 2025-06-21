using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
{
    private  readonly ApplicationDbContext _context;
    public VillaNumberRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(VillaNumber villaNumber)
    {
        _context.VillaNumbers.Update(villaNumber);
    }
}