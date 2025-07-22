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

    public void Add(Booking booking)
    {
        _unitOfWork.Bookings.Add(booking);
        _unitOfWork.Save();
    }
}