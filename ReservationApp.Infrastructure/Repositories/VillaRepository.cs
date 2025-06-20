using System.Linq.Expressions;
using Microsoft.VisualBasic;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Infrastructure.Repositories;

public class VillaRepository: IVillaRepository
{
    public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, Strings? includeProperty = null)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Villa> Get(Expression<Func<Villa, bool>> filter, Strings? includeProperty = null)
    {
        throw new NotImplementedException();
    }

    public void Add(Villa villa)
    {
        throw new NotImplementedException();
    }

    public void Update(Villa villa)
    {
        throw new NotImplementedException();
    }

    public void Delete(Villa villa)
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }
}