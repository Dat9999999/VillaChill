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
}