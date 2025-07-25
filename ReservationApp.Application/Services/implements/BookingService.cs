using System.Data;
using System.Linq.Expressions;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.implements;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IEnumerable<Booking> GetAll(Expression<Func<Booking, bool>>? filter = null,string includeProperties = "")
    {
        return _unitOfWork.Bookings.GetAll(filter, includeProperties);
    }

    public Booking GetById(Expression<Func<Booking, bool>>? filter = null, string includeProperties = "")
    {
        return _unitOfWork.Bookings.Get(filter, includeProperties);       
    }

    public void UpdateStatus(int bookingId, string status)
    {
        _unitOfWork.Bookings.UpdateStatus(bookingId, status);
        _unitOfWork.Save();
    }

    public void UpdatePaymentId(int bookingId, string PaymentId)
    {
        _unitOfWork.Bookings.UpdatePaymentId(bookingId, PaymentId);
        _unitOfWork.Save();
    }

    public void Add(Booking booking, string paymentMethod)
    {

        using var transaction = _unitOfWork.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            
            // 1. check available room in villa
            var overlappingBooking = _unitOfWork.Bookings.Any(b =>
                b.VillaId == booking.VillaId && booking.VillaNumber == b.VillaNumber &&
                (
                    (booking.CheckInDate >= b.CheckInDate && booking.CheckInDate < b.CheckOutDate) ||
                    (booking.CheckOutDate > b.CheckInDate && booking.CheckOutDate <= b.CheckOutDate) ||
                    (booking.CheckInDate <= b.CheckInDate && booking.CheckOutDate >= b.CheckOutDate)
                )
            );

            if (overlappingBooking)
            {
                throw new Exception("Room is already booked. Please try another room or villa.");
            }
            // 2. Add booking
            var villa = _unitOfWork.Villas.Get(x => x.Id == booking.VillaId);;
            booking.TotalCost = villa.Price * booking.Nights;
        
            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;
            booking.IsPaidAtCheckIn = paymentMethod == SD.PaymentMethod_Onsite;

            _unitOfWork.Bookings.Add(booking);

            // 3. Save and commit
            _unitOfWork.Save();
            transaction.Commit();       
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }


    public IDisposable BeginTransaction(IsolationLevel serializable)
    {
        throw new NotImplementedException();
    }
}