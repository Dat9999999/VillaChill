using System.Linq.Expressions;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface IVillaService
{
    public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string includeProperties = "");
    public Villa GetById(int id, string includeProperties = "");
    public void Update(Villa villa);
    public bool Delete(Villa villa, out string errorMessage);
    public bool Add(Villa villa, out string errorMessage);

}