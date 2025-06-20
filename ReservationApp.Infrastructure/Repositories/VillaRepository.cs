using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class VillaRepository: Repository<Villa>, IVillaRepository
{
    private  readonly ApplicationDbContext _context;
    public VillaRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
   

    public void Update(Villa villa)
    {
        _context.Villas.Update(villa);
    }

    public void Save()
    {
        _context.SaveChanges();   
    }
}