using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;
using ReservationApp.Infrastructure.Email;
using ReservationApp.Infrastructure.Exporting;
using ReservationApp.Infrastructure.Payments;
using ReservationApp.Infrastructure.Repositories;

namespace ReservationApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

       services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
       services.AddScoped<IUnitOfWork, UnitOfWork>();


        // connect vnpay
       services.AddScoped<IVnPayService, VnPayService>();

        //export file 
       services.AddScoped<IExporter, Exporter>();


        //intialDB 
       services.AddScoped<IDbInitializer, DbInitializer>();
       
       //email 
       services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}