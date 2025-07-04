namespace ReservationApp.Application.Common.Interfaces;

public interface IUnitOfWork
{
    public IVillaRepository Villas { get; }
    public IVillaNumberRepository VillaNumbers { get; }
    public IAmenityRepository Amenities { get; }
    public IBookingRepository Bookings { get; }
    public IApplicationUserRepository ApplicationUsers { get; }
    void Save();
}