namespace ReservationApp.ViewModels;

public class RatingRequestDTO
{
    public int BookingId { get; set; }
    public int Score { get; set; } // from 0-10
    public string? Comment { get; set; }
}