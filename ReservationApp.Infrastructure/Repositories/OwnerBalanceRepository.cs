using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class OwnerBalanceRepository : Repository<OwnerBalance>, IOwnerBalanceRepository
{
    private  readonly ApplicationDbContext _context;
    public OwnerBalanceRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
        
    }

    public void Update(OwnerBalance ownerBalance)
    {
        _context.OwnerBalances.Update(ownerBalance);
    }
}