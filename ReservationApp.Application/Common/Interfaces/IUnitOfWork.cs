namespace ReservationApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villas { get; }
    public IVillaNumberRepository VillaNumbers { get; }
    public IAmenityRepository Amenities { get; }
    void Save();
}