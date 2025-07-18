using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class CommissionRateRepository : Repository<CommissionRate>, ICommissionRateRepository
{
    private  readonly ApplicationDbContext _context;
    public CommissionRateRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(CommissionRate commissionRate)
    {
        _context.CommissionRates.Update(commissionRate);
    }
}