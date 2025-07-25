using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.interfaces;

public interface IOwnerSettlementService
{
    // create when customer pay onsite
    public void Create(Booking booking);

    // update when owner pay for admin or overdue
    void UpdatePaymentStatus(IEnumerable<int> bookingId,string statusPayment);
    public IEnumerable<OwnerSettlementDTO?> GetAll(string? UserId, bool isAdmin);
    PaymentInformationModel MarkAsPaidBulk(List<int> bookingIds);
    void RestrictOverdue(List<string> ownerIds);
}
