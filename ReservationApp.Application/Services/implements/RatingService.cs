using System.Linq.Expressions;
using AutoMapper;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.implements;

public class RatingService : IRatingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;       
    }
    public IEnumerable<Rating> GetAll(Expression<Func<Rating, bool>>? filter = null, string includeProperties = "")
    {
        var ratings = _unitOfWork.Ratings.GetAll(filter, includeProperties);
        return ratings;       
    }

    public Rating GetById(Expression<Func<Rating, bool>>? filter = null, string includeProperties = "")
    {
        throw new NotImplementedException();
    }

    public void UpdateStatus(int RatingId, string status, int villaNumber)
    {
        throw new NotImplementedException();
    }

    public void UpdatePaymentId(int RatingId, string PaymentId)
    {
        throw new NotImplementedException();
    }

    public void Add(RatingRequestDTO RatingDto)
    {
        var rating = _mapper.Map<Rating>(RatingDto);
        _unitOfWork.Ratings.Add(rating);
        _unitOfWork.Save();   
    }
}