using Hangfire;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;

namespace ReservationApp.Infrastructure.SystemJob;

public class BackgroundJobScheduler : IBackgroundJobScheduler
{
    public void ScheduleCancelBooking(int bookingId, TimeSpan delay)
    {
        BackgroundJob.Schedule<IBookingService>(
            service => service.CancelBooking(bookingId),
            delay);
    }
}