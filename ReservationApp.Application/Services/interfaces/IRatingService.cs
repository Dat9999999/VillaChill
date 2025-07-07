using System.Linq.Expressions;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.interfaces;

public interface IRatingService
{
    public IEnumerable<Rating> GetAll(Expression<Func<Rating, bool>>? filter = null, string includeProperties = "");

    public Rating GetById(Expression<Func<Rating, bool>>? filter = null, string includeProperties = "");
    public void UpdateStatus(int RatingId, string status, int villaNumber);
    void UpdatePaymentId(int RatingId, string PaymentId);
    void Add(RatingRequestDTO Rating);
    
}