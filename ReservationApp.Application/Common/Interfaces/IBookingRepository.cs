using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    public void Update(Booking Booking);
}