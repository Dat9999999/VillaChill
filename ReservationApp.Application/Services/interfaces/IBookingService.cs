using System.Linq.Expressions;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface IBookingService
{
    public IEnumerable<Booking> GetAll(Expression<Func<Booking, bool>>? filter = null, string includeProperties = "");

    public Booking GetById(Expression<Func<Booking, bool>>? filter = null, string includeProperties = "");
    public void UpdateStatus(int bookingId, string status);
    void UpdatePaymentId(int bookingId, string PaymentId);
    void Add(Booking booking);
}