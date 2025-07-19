using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IOwnerBalanceRepository : IRepository<OwnerBalance>
{
    void Update(OwnerBalance ownerBalance);
}