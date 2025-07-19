using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.implements;

public class OwnerBalanceService: IOwnerBalanceService
{
    private readonly IUnitOfWork _unitOfWork;
    public OwnerBalanceService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public OwnerBalance Create(string ownerEmail)
    {
        var obj = new OwnerBalance();
        obj.OwnerEmail = ownerEmail;
        _unitOfWork.OwnerBalances.Add(obj);   
        _unitOfWork.Save();
        return obj;       
    }

    public void UpdateBalance(int bookingId, double bookingTotalCost)
    {
        var booking = _unitOfWork.Bookings.Get(x => x.Id == bookingId, "Villa");;
        var ownerEmail = booking.Villa.OwnerEmail;
        var ownerBalance = _unitOfWork.OwnerBalances.Get(x => x.OwnerEmail == ownerEmail);
        if (ownerBalance is not null)
        {
            ownerBalance.CurrentBalance += bookingTotalCost;
            ownerBalance.TotalEarned += bookingTotalCost;
            _unitOfWork.OwnerBalances.Update(ownerBalance);
            _unitOfWork.Save();       
        }
    }
}