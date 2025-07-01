using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface IVillaService
{
    public IEnumerable<Villa> GetAll();
    public Villa GetById(int id);
    public void Update(Villa villa);
    public void Delete(Villa villa);
    public void Add(Villa villa);
}