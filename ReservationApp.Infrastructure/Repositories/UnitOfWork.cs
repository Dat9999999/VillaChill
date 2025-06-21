using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IVillaRepository Villas { get; }
    public IVillaNumberRepository VillaNumbers { get; }

    public void Save()
    {
        _context.SaveChanges();   
    }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Villas = new VillaRepository(context);
        VillaNumbers = new VillaNumberRepository(context);
    }
}