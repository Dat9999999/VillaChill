using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;

namespace ReservationApp.Infrastructure.Repositories;

public class VillaRepository: IVillaRepository
{
    private  readonly ApplicationDbContext _context;
    public VillaRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<Villa> query = _context.Set<Villa>();
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        return query.ToList();
    }

    public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
    {
        IQueryable<Villa> query = _context.Set<Villa>();
        if (filter is not null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }
        return query.FirstOrDefault();
    }

    public void Add(Villa villa)
    {
        _context.Villas.Add(villa);
    }

    public void Update(Villa villa)
    {
        _context.Villas.Update(villa);
    }

    public void Delete(Villa villa)
    {
        _context.Villas.Remove(villa);
    }

    public void Save()
    {
        _context.SaveChanges();   
    }
}