using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    public void Update(Booking Booking);
    public void UpdateStatus(int bookingId, string status);
    public void UpdatePaymentId(int bookingId,  string paymentId);
}