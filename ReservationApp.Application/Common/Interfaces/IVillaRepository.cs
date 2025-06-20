using System.Linq.Expressions;
using Microsoft.VisualBasic;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IVillaRepository : IRepository<Villa>
{
    void Update(Villa villa);
    void Save();
}