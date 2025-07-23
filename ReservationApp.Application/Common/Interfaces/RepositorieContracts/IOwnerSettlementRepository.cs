using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IOwnerSettlementRepository : IRepository<OwnerSettlement>
{
    void Update(OwnerSettlement ownerSettlement);
}