namespace ReservationApp.ViewModels;

public class UserManagementVM
{
    public IEnumerable<UserVM> Users { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
}