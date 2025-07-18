using System.Linq.Expressions;
using AutoMapper;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.implements;

public class ComissionService : IComissionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ComissionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;      
    }
    public IEnumerable<CommissionRate> GetAll(Expression<Func<CommissionRate, bool>>? filter = null, string includeProperties = "")
    {
        return _unitOfWork.CommissionRates.GetAll(filter, includeProperties);       
    }

    public CommissionRate Add(CommissionRateRequestDTO commissionRateRequestDto)
    {
        var commissionRate = _mapper.Map<CommissionRate>(commissionRateRequestDto);
        _unitOfWork.CommissionRates.Add(commissionRate);
        _unitOfWork.Save();
        return commissionRate;       
    }

    public CommissionRate Get(Expression<Func<CommissionRate, bool>>? filter = null, string includeProperties = "")
    {
        var commissionRate = _unitOfWork.CommissionRates.Get(filter, includeProperties);
        return commissionRate;       
    }

    public void Update(CommissionRate commissionRate)
    {
        _unitOfWork.CommissionRates.Update(commissionRate);
        _unitOfWork.Save();
    }

    public void Delete(CommissionRate commissionRate)
    {
        _unitOfWork.CommissionRates.Delete(commissionRate);
        _unitOfWork.Save();       
    }
}