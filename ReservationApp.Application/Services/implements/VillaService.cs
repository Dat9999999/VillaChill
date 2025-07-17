using System.Linq.Expressions;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ReservationApp.Application.Services.implements;

public class VillaService : IVillaService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;
    public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        _unitOfWork = unitOfWork;
        _env = env;       
        
    }
    public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter,string includeProperties)
    {
        return _unitOfWork.Villas.GetAll(filter,includeProperties: includeProperties);;
    }

    public Villa GetById(int id, string includeProperties = "")
    {
        return _unitOfWork.Villas.Get(x => x.Id == id, includeProperties);
    }

    public void Update(Villa villa)
    {
        if (villa.Image is not null)
        {
            string fileName = Guid.NewGuid() + Path.GetFileName(villa.Image.FileName);
            string filePath = Path.Combine(_env.WebRootPath, @"images/VillaImage", fileName);
            if (!string.IsNullOrEmpty(villa.ImageUrl))
            {
                var filePathToDelete = Path.Combine(_env.WebRootPath, villa.ImageUrl.TrimStart('/'));
                if (File.Exists(filePathToDelete))
                {
                    File.Delete(filePathToDelete);
                }
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                villa.Image.CopyTo(stream);
            }
            villa.ImageUrl = @"/images/VillaImage/" + fileName;
        }
        _unitOfWork.Villas.Update(villa);
        _unitOfWork.Save();
    }

    public bool Delete(Villa villa, out string errorMessage)
    {
        var villaToDelete = _unitOfWork.Villas.Get(x => x.Id == villa.Id);
        if (villaToDelete is null)
        {
            errorMessage = "Villa not found";
            return false;       
        }
        if (!string.IsNullOrEmpty(villaToDelete.ImageUrl))
        {
            var filePathToDelete = Path.Combine(_env.WebRootPath, villaToDelete.ImageUrl.TrimStart('/'));
            if (File.Exists(filePathToDelete))
            {
                File.Delete(filePathToDelete);
            }
        }
        _unitOfWork.Villas.Delete(villaToDelete);
        _unitOfWork.Save();
        errorMessage = string.Empty;
        return true;
    }

    public bool Add(Villa villa, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (villa.Name == villa.Description)
        {
            errorMessage = "Name and Description cannot be the same";
            return false;
        }
        
        if (villa.Image is not null)
        {
            string fileName = Guid.NewGuid() + ( Path.GetFileName(villa.Image.FileName));
            string filePath = Path.Combine(_env.WebRootPath, @"images/VillaImage", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                villa.Image.CopyTo(stream);
            }
            villa.ImageUrl = @"/images/VillaImage/" + fileName;
        }
        else
        {  
            villa.ImageUrl = @"/images/VillaImage/placeholder.png";
        }
        _unitOfWork.Villas.Add(villa);
        _unitOfWork.Save();
        return true;
    }
}