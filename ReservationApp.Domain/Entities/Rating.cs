using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ReservationApp.Domain.Entities;

public class Rating
{
    public int Id { get; set; }
    [ForeignKey(nameof(BookingId))]
    public int BookingId { get; set; }
    [NotMapped]
    public Booking Booking { get; set; }
    public string CustomerName { get; set; }
    [ForeignKey(nameof(VillaId))]
    public int VillaId { get; set; }
    [ValidateNever]
    public Villa Villa { get; set; }
    [Required]  
    public double Score { get; set; }
    public string? Comment { get; set; }
    public DateTime Date { get; set; }
}