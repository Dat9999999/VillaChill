using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IRatingRepository : IRepository<Rating>
{
    public void Update(Rating rating);
}