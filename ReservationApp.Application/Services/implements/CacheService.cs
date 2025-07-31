using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.implements;

public class CacheService:ICacheService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;
    public CacheService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;      
    }
    public IEnumerable<Booking> GetBookings(string userId, string status)
    {
        string key = $"Bookings_Of_{userId}_{status}";
        if (!_memoryCache.TryGetValue(key, out IEnumerable<Booking> bookings))
        {
            bookings = _unitOfWork.Bookings.GetAll(x => x.UserId == userId && x.Status == status);
            _memoryCache.Set(key, bookings, new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            );
        }
        return bookings;       
    }

    public IEnumerable<Villa> GetVillas(string? city, string includeProperties, bool isTracked ,int? page, int? pageSize)
    {
        var cityKey = string.IsNullOrEmpty(city) ? "AllCities" : city;
        var key = $"Villas_City_{cityKey}_Include_{includeProperties}_Page_{page}_Size_{pageSize}";

        Expression<Func<Villa, bool>>? filter = null;

        if (!string.IsNullOrEmpty(city))
        {
            filter = x => x.City == city;
        }
        if (!_memoryCache.TryGetValue(key, out IEnumerable<Villa> villas))
        {
            villas = _unitOfWork.Villas.GetAll(filter, includeProperties: includeProperties, isTracked,page, pageSize);
            _memoryCache.Set(key, villas, new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10))
            );
        }
        return villas;
    }
}