using System.ComponentModel.DataAnnotations;

namespace ReservationApp.ViewModels;

public class CommissionRateRequestDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public double Rate { get; set; }
    public string? Description { get; set; }
}