using System.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace ReservationApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villas { get; }
    public IVillaNumberRepository VillaNumbers { get; }
    public IAmenityRepository Amenities { get; }
    public IBookingRepository Bookings { get; }
    public IApplicationUserRepository ApplicationUsers { get; }
    public IRatingRepository Ratings { get; }
    public ICommissionRateRepository CommissionRates { get; }
    public IOwnerBalanceRepository OwnerBalances { get; }
    public IOwnerSettlementRepository OwnerSettlements { get; }
    IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    void Save();
}