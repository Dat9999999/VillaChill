namespace ReservationApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villas { get; }
}