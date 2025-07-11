using Microsoft.AspNetCore.Identity;

namespace ReservationApp.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public bool IsOwner { get; set; }
    public DateTime CreatedAt { get; set; }
}