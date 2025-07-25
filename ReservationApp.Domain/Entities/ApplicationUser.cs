using Microsoft.AspNetCore.Identity;

namespace ReservationApp.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public bool? isOverDue { get; set; } = false;
    public DateTime CreatedAt { get; set; }
}