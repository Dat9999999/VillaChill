using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class OwnerSettlementRepository : Repository<OwnerSettlement>, IOwnerSettlementRepository
{
    private  readonly ApplicationDbContext _context;
    public OwnerSettlementRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OwnerSettlement ownerSettlement)
    {
        _context.OwnerSettlements.Update(ownerSettlement);
    }
}