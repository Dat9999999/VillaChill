using System.Linq.Expressions;
using Microsoft.VisualBasic;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IVillaNumberRepository : IRepository<VillaNumber>
{
    void Update(VillaNumber villaNumber);
}