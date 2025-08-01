using Microsoft.AspNetCore.Identity;

namespace ReservationApp.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public bool? isOverDue { get; set; } = false;
    public string? AvatarUrl { get; set; } = "/images/Avatar/default.jpg";
    public DateOnly? LastLoginDate { get; set; }
    public DateTime CreatedAt { get; set; }
}