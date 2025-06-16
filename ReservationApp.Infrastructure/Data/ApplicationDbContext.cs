using Microsoft.EntityFrameworkCore;

namespace ReservationApp.Infrastructure.Data;

public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}