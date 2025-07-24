using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.interfaces;

public interface IOwnerSettlementService
{
    // create when customer pay onsite
    public void Create(Booking booking);

    // update when owner pay for admin or overdue
    void Update(int bookingId, string statusPayment);
}
