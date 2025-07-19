using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IVillaRepository Villas { get; }
    public IVillaNumberRepository VillaNumbers { get; }
    public IAmenityRepository Amenities { get; }
    public IBookingRepository Bookings { get; }
    public IApplicationUserRepository ApplicationUsers { get; }
    public IRatingRepository Ratings { get; }
    public IOwnerBalanceRepository OwnerBalances { get; }
    public ICommissionRateRepository CommissionRates { get; }

    public void Save()
    {
        _context.SaveChanges();   
    }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Villas = new VillaRepository(context);
        VillaNumbers = new VillaNumberRepository(context);
        Amenities = new AmenityRepository(context);
        Bookings = new BookingRepository(context);
        ApplicationUsers = new ApplicationUserRepository(context);
        Ratings = new RatingRepository(context);
        CommissionRates = new CommissionRateRepository(context);
        OwnerBalances = new OwnerBalanceRepository(context);

    }
}