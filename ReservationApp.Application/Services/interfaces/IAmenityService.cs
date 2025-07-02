using System.Linq.Expressions;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface IAmenityService
{
    public IEnumerable<Amenity> GetAll(Expression<Func<Amenity, bool>> filter = null, string includeProperties = "");
    public Amenity GetById(int amenityId);
    public bool IsExist(int amenityId);
    public void Update(Amenity amenity);
    public bool Delete(Amenity amenity, out string errorMessage);
    public bool Add(Amenity amenity, out string errorMessage);
    
}