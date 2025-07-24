using AutoMapper;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.implements;

public class OwnerSettlementService : IOwnerSettlementService
{
    private IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public OwnerSettlementService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;      
    }
    public void Create(Booking booking)
    {
        // _unitOfWork.OwnerSettlements.Add();
        
        //get ownerID
        var villa = _unitOfWork.Villas.Get(x => x.Id == booking.VillaId);
        var Owner = _unitOfWork.ApplicationUsers.Get(x => x.Email == villa.OwnerEmail).Id;

        var commissionRate = _unitOfWork.CommissionRates.Get(x => x.Name == SD.CommissionRate_platform).Rate;
        var newOwnerSettlement = new OwnerSettlementDTO()
        {
            BookingId = booking.Id,
            OwnerId = Owner,
            Amount = booking.TotalCost,
            CommissionRate = commissionRate,
            Status = SD.StatusPayment_Unpaid,
            DueDate = DateTime.Now.AddDays(SD.DueDate)
        };
        var ownerSettlement = _mapper.Map<OwnerSettlement>(newOwnerSettlement);
        _unitOfWork.OwnerSettlements.Add(ownerSettlement);
        _unitOfWork.Save();       

    }

    public void Update(int bookingId, string statusPayment)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OwnerSettlementDTO?> GetAll(string? OwnerId, bool isAdmin)
    {
        IEnumerable<OwnerSettlement> ownerSettlements;
        if (isAdmin)
        {
            ownerSettlements = _unitOfWork.OwnerSettlements.GetAll();
        }
        else
        {
            ownerSettlements = _unitOfWork.OwnerSettlements.GetAll(x => x.OwnerId == OwnerId);
        }
        return _mapper.Map<IEnumerable<OwnerSettlementDTO>>(ownerSettlements);       
    }
}