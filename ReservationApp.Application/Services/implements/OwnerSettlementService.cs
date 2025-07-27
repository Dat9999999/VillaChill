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
    private readonly IEmailService _emailService;
    public OwnerSettlementService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;      
        _emailService = emailService;      
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

    public void UpdatePaymentStatus(IEnumerable<int> bookingId, string statusPayment)
    {
        foreach (var booking in bookingId)
        {
            var ownerSettlement = _unitOfWork.OwnerSettlements.Get(x => x.BookingId == booking);
            ownerSettlement.Status = statusPayment;
            _unitOfWork.OwnerSettlements.Update(ownerSettlement);       
        }
        _unitOfWork.Save();       
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

    //call this after paid successfully
    public PaymentInformationModel MarkAsPaidBulk(List<int> bookingIds)
    {   
        var ownerSettlements = _unitOfWork.OwnerSettlements.GetAll()
            .Where(s =>bookingIds.Contains(s.BookingId) && s.Status != SD.StatusPayment_Paid);
        var model = new PaymentInformationModel
        {
            Amount = ownerSettlements.Sum(s => (double)(s.Amount *s.CommissionRate)/100),
            Name = "Owner",
            OrderDescription = $"{string.Join(",", bookingIds)}",
            OrderType = "ownerSettlement",
        };
        return model;
    }

    public void RestrictOverdue(List<string> ownerIds)
    {
        var owners = _unitOfWork.ApplicationUsers.GetAll()
            .Where(s => ownerIds.Contains(s.Id)).ToList();
        foreach (var owner in owners)
        {
            owner.isOverDue = true;
            _unitOfWork.ApplicationUsers.Update(owner);       
        }
        _unitOfWork.Save();       
    }

    public IEnumerable<string> GetOverdueOwnerIds()
    {
        var today = DateTime.Now;
        var ownersOverdue = _unitOfWork.OwnerSettlements
            .GetAll(x => x.DueDate < today && x.Status != SD.StatusPayment_Paid)
            .GroupBy(x => x.OwnerId)
            .Select(g => g.Key).ToList();
        return ownersOverdue;       
    }

    public void RestrictOwnerAutomatically()
    {
        var ownersOverdue = GetOverdueOwnerIds();
        if (ownersOverdue.Any())
        {
            foreach (var ownerId in ownersOverdue)
            {
                var owner = _unitOfWork.ApplicationUsers.Get(x => x.Id == ownerId);
                //check if owner is already restricted
                if (owner.isOverDue != true)
                {
                    owner.isOverDue = true;
                    _unitOfWork.ApplicationUsers.Update(owner);  
                    
                    //send notification to owner 
                    _emailService.SendEmail(owner.Email,
                        "You have overdue platform fees",
                        $"Dear {owner.Name ?? owner.Email}," +
                        $"\n\nYour account has been restricted due to unpaid fees. Please complete your payment to continue using the service.\n\nThank you.");
                }
            }
            _unitOfWork.Save();       
        }
    }
}