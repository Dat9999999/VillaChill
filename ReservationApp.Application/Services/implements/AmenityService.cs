using System.Linq.Expressions;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.implements;

public class AmenityService : IAmenityService
{
    private readonly IUnitOfWork _unitOfWork;
    public AmenityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IEnumerable<Amenity> GetAll(Expression<Func<Amenity, bool>> filter = null ,string includeProperties = "")
    {
        return _unitOfWork.Amenities.GetAll(null, includeProperties);
    }

    public Amenity GetById(int amenityId)
    {
        return _unitOfWork.Amenities.Get(x => x.Id == amenityId);       
    }

    public bool IsExist(int amenityId)
    {
        return _unitOfWork.Amenities.Any(u => u.Id == amenityId);
    }

    public void Update(Amenity amenity)
    {
        _unitOfWork.Amenities.Update(amenity);
        _unitOfWork.Save();
    }

    public bool Delete(Amenity amenity, out string errorMessage)
    {
        var objToDelete = _unitOfWork.Amenities.Get(x => x.Id== amenity.Id);
        if (objToDelete is null)
        {
            errorMessage = "Amenity not found";
            return false;       
        }
        _unitOfWork.Amenities.Delete(objToDelete);
        _unitOfWork.Save();
        errorMessage = string.Empty;
        return true;       
    }

    public bool Add(Amenity amenity, out string errorMessage)
    {
        if (IsExist(amenity.Id))
        {
            errorMessage = "Amenity already exists";
            return false;       
        }
        _unitOfWork.Amenities.Add(amenity);
        _unitOfWork.Save();
        errorMessage = string.Empty;
        return true;
    }

    
}