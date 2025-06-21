namespace ReservationApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villas { get; }
    public IVillaNumberRepository VillaNumbers { get; }
    void Save();
}