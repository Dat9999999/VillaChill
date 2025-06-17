using System.ComponentModel;

namespace ReservationApp.Domain.Entities;

public class Villa
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Sqft { get; set; }
    public int Occupancy { get; set; }
    [DisplayName( "Price per night")]
    public double Price { get; set; }
    [DisplayName( "Image Url")]
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}