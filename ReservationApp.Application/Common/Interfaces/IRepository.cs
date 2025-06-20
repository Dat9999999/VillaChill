using System.Linq.Expressions;

namespace ReservationApp.Application.Common.Interfaces;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
    T Get(Expression<Func<T, bool>> filter, string? includeProperty = null);
    void Add(T obj);
    void Delete(T obj);
}