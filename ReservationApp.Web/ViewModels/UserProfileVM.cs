namespace ReservationApp.Models;

public class UserProfileVM
{
    public string Name { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsLocked { get; set; }
}