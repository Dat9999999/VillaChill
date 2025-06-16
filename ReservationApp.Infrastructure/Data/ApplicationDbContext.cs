using Microsoft.EntityFrameworkCore;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Infrastructure.Data;

public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Villa> Villas { get; set; }
}