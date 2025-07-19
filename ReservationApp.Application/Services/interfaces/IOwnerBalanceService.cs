using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface IOwnerBalanceService
{
    public OwnerBalance Create(string ownerEmail);
    void UpdateBalance(int bookingId);
}