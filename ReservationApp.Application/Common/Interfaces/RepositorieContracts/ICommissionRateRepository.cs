using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface ICommissionRateRepository : IRepository<CommissionRate>
{
    void Update(CommissionRate commissionRate);
    
}