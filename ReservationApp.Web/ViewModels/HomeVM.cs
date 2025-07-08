using System.ComponentModel.DataAnnotations;
using ReservationApp.Domain.Entities;

namespace ReservationApp.ViewModels;

public class HomeVM
{
    public IEnumerable<Villa> VillaList { get; set; }
    public IEnumerable<VillaNumber> VillaNumberList { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly? CheckOutDate { get; set; }
    [Required]
    public string City { get; set; }
    public int Nights { get; set; }
}