using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class BookingRepository: Repository<Booking>, IBookingRepository
{
    private  readonly ApplicationDbContext _context;
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Booking Booking)
    {
        _context.Bookings.Update(Booking);
    }
}