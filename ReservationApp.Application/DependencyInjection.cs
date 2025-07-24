using Microsoft.Extensions.DependencyInjection;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.mappers;
using ReservationApp.Application.Services.implements;
using ReservationApp.Application.Services.interfaces;

namespace ReservationApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
       services.AddScoped<IDashboardService, DashboardService>();
       services.AddScoped<IVillaService, VillaService>();
       services.AddScoped<IVillaNumberService, VillaNumberService>();
       services.AddScoped<IAmenityService, AmenityService>();
       services.AddScoped<IBookingService, BookingService>();
       services.AddScoped<IRatingService, RatingService>();
       services.AddScoped<IComissionService, ComissionService>();
       services.AddScoped<IOwnerBalanceService, OwnerBalanceService>();
       services.AddScoped<IOwnerSettlementService,OwnerSettlementService>();
       services.AddAutoMapper(typeof(MappingProfile));
        return services;
    }
}