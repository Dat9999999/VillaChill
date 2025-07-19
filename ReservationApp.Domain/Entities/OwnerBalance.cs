using System.ComponentModel.DataAnnotations;

namespace ReservationApp.Domain.Entities;

public class OwnerBalance
{
    public int Id { get; set; }
    [Required]
    public string OwnerEmail { get; set; }

    public double CurrentBalance { get; set; } = 0;
    public double  TotalEarned { get; set; } = 0;
}