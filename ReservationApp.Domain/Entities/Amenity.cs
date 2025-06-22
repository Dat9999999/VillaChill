using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ReservationApp.Domain.Entities;

public class Amenity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    [ForeignKey("Villa")]
    public int VillaId { get; set; }
    [ValidateNever]
    public Villa Villa { get; set; }
}