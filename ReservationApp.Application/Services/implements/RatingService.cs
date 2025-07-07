using System.Linq.Expressions;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.implements;

public class RatingService : IRatingService
{
    private readonly IUnitOfWork _unitOfWork;
    public RatingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IEnumerable<Rating> GetAll(Expression<Func<Rating, bool>>? filter = null, string includeProperties = "")
    {
        throw new NotImplementedException();
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
        // mappper 
        Rating rating = new Rating()
        {
            BookingId = RatingDto.BookingId,
            Comment = RatingDto.Comment,
            Date = DateTime.Now,
            Score = RatingDto.Score
        };
        _unitOfWork.Ratings.Add(rating);
        _unitOfWork.Save();   
    }
}