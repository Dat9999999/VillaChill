using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.interfaces;

public interface ICacheService
{
    public IEnumerable<Booking> GetBookings(string userId, string status);
    public IEnumerable<Villa> GetVillas(string? city,string includeProperties,bool isTracked ,int? page, int? pageSize);
}