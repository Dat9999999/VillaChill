using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface IVillaService
{
    public IEnumerable<Villa> GetAll();
    public Villa GetById(int id);
    public void Update(Villa villa);
    public bool Delete(Villa villa, out string errorMessage);
    public bool Add(Villa villa, out string errorMessage);
}