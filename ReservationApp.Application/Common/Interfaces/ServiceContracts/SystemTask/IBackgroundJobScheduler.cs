namespace ReservationApp.Application.Common.Interfaces;

public interface IBackgroundJobScheduler
{
    void ScheduleCancelBooking(int bookingId, TimeSpan delay);
}