using Hangfire;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;

namespace ReservationApp.Infrastructure.SystemJob;

public class JobScheduler:IJobScheduler
{
    public void ScheduleJob()
    {
        // for testing use minutely
        RecurringJob.AddOrUpdate<IOwnerSettlementService>(
            x => x.RestrictOwnerAutomatically(),
            Cron.Minutely
            // Cron.Daily
            );
        RecurringJob.AddOrUpdate<IDashboardService>(
            x => x.exportRevenueReport(),
            // Cron.Minutely
            Cron.Weekly
            );
    }
}