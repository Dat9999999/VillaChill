using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    public void Update(ApplicationUser applicationUser);
}