using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Domain.Entities;

public class CommissionRate
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Rate { get; set; }
    public string? Description { get; set; }
}