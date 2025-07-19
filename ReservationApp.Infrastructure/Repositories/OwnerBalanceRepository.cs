using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class OwnerBalanceRepository : Repository<OwnerBalance>, IOwnerBalanceRepository
{
    public OwnerBalanceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public void Update(OwnerBalance ownerBalance)
    {
        throw new NotImplementedException();
    }
}