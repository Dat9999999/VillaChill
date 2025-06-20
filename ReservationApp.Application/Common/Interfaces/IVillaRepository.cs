using System.Linq.Expressions;
using Microsoft.VisualBasic;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IVillaRepository
{
    IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, Strings? includeProperty = null);
    IEnumerable<Villa> Get(Expression<Func<Villa, bool>> filter, Strings? includeProperty = null);
    void Add(Villa villa);
    void Update(Villa villa);
    void Delete(Villa villa);
    void Save();
}