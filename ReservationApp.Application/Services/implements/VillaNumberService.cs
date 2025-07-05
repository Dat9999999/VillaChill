using System.Linq.Expressions;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.implements;

public class VillaNumberService: IVillaNumberService
{
    private readonly IUnitOfWork _unitOfWork;
    public VillaNumberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IEnumerable<VillaNumber> GetAll(Expression<Func<VillaNumber, bool>>? filter = null, string includeProperties = "")
    {
        return _unitOfWork.VillaNumbers.GetAll(filter, "Villa");
    }

    public VillaNumber GetById(int villaNumber)
    {
        return _unitOfWork.VillaNumbers.Get(u => u.Villa_Number == villaNumber);
    }

    public bool IsExist(int villaNumber)
    {
        return _unitOfWork.VillaNumbers.Any(u => u.Villa_Number == villaNumber);
    }

    public void Update(VillaNumber villaNumber)
    {
        _unitOfWork.VillaNumbers.Update(villaNumber);
        _unitOfWork.Save();
    }

    public bool Delete(VillaNumber villaNumber, out string errorMessage)
    {
        var objToDelete = _unitOfWork.VillaNumbers.Get(x => x.Villa_Number== villaNumber.Villa_Number);
        if (objToDelete is not null)
        {
            _unitOfWork.VillaNumbers.Delete(objToDelete);
            _unitOfWork.Save();
            errorMessage = string.Empty;
            return true;
        } 
        errorMessage = "Villa not found";
        return false;
    }

    public bool Add(VillaNumber villaNumber, out string errorMessage)
    {
        var isExists = IsExist(villaNumber.Villa_Number);
        if (isExists)
        {
            errorMessage = "Villa number is already exists";
            return false;       
        }
        _unitOfWork.VillaNumbers.Add(villaNumber);
        _unitOfWork.Save();
        errorMessage = string.Empty;
        return true;
    }
}