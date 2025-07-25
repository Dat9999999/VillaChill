using System.Linq.Expressions;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IVillaNumberService
{
    public IEnumerable<VillaNumber> GetAll(Expression<Func<VillaNumber, bool>>? filter = null, string includeProperties = "");
    public VillaNumber GetById(int villaNumber);
    public bool IsExist(int villaNumber);
    public void Update(VillaNumber villaNumber);
    public bool Delete(VillaNumber villaNumber, out string errorMessage);
    public bool Add(VillaNumber villaNumber, out string errorMessage);
}