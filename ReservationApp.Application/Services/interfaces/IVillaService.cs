using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface IVillaService
{
    public IEnumerable<Villa> GetAll(string includeProperties = "");
    public Villa GetById(int id, string includeProperties = "");
    public void Update(Villa villa);
    public bool Delete(Villa villa, out string errorMessage);
    public bool Add(Villa villa, out string errorMessage);
}