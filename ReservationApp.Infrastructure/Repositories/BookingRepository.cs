using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
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

    public void UpdateStatus(int bookingId, string status)
    {
        var bookingFromDB = _context.Bookings.FirstOrDefault(x => x.Id == bookingId);
        if (bookingFromDB is not null)
        {
            bookingFromDB.Status = status;
            if (status == SD.StatusCheckedIn)
            {
                bookingFromDB.ActualCheckInDate = DateTime.Now;
            }

            if (status == SD.StatusCompleted)
            {
                bookingFromDB.ActualCheckOutDate = DateTime.Now;
            }
        }
        
    }

    public void UpdatePaymentId(int bookingId, string sessionId, string paymentId)
    {
        var bookingFromDB = _context.Bookings.FirstOrDefault(x => x.Id == bookingId);
        if (bookingFromDB is not null)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                bookingFromDB.StripeSessionId = sessionId;
            }

            if (!string.IsNullOrEmpty(paymentId))
            {
                bookingFromDB.IsPaymentSuccessful = true;
                // bookingFromDB.StripePaymentIntentId = paymentId;
                bookingFromDB.PaymentDate = DateTime.Now;
            }
        }
    }
}