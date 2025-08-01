namespace ReservationApp.ViewModels;

public class UserVM
{
    public string Id { get; set; }
    public string? AvatarUrl { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateOnly LastLoginTime { get; set; }
    public bool IsLocked { get; set; }
}