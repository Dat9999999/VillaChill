using Hangfire;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;

namespace ReservationApp.Infrastructure.SystemJob;

public class JobScheduler:IJobScheduler
{
    public void ScheduleJob()
    {
        RecurringJob.AddOrUpdate<IOwnerSettlementService>(
            x => x.RestrictOwnerAutomatically(),
            Cron.Minutely
            );
        RecurringJob.AddOrUpdate<IDashboardService>(
            x => x.exportRevenueReport()
            , Cron.Minutely);
    }
}