using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class ApplicationUserRepository: Repository<ApplicationUser>, IApplicationUserRepository
{
    private  readonly ApplicationDbContext _context;
    public ApplicationUserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(ApplicationUser applicationUser)
    {
        _context.ApplicationUsers.Update(applicationUser);
    }
}