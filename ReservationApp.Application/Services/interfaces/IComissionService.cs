using System.Linq.Expressions;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.interfaces;

public interface IComissionService
{
    public IEnumerable<CommissionRate> GetAll(Expression<Func<CommissionRate, bool>>? filter = null, string includeProperties = "");
    CommissionRate Add(CommissionRateRequestDTO commissionRateRequestDto);
    public CommissionRate Get(Expression<Func<CommissionRate, bool>>? filter = null, string includeProperties = "");
    void Update(CommissionRate commissionRate);
    void Delete(CommissionRate commissionRate);
}