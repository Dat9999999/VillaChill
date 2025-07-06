using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationApp.Domain.Entities;

public class Rating
{
    public int Id { get; set; }
    [ForeignKey(nameof(BookingId))]
    public int BookingId { get; set; }
    [NotMapped]
    public Booking Booking { get; set; }
    [Required]  
    public double Score { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; }
}