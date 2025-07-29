using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;
using ReservationApp.Infrastructure.Email;
using ReservationApp.Infrastructure.Exporting;
using ReservationApp.Infrastructure.Payments;
using ReservationApp.Infrastructure.Repositories;
using ReservationApp.Infrastructure.SystemJob;

namespace ReservationApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
       
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
         {
          options.Password.RequireDigit = true;
          options.Password.RequiredLength = 6;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = true;
          options.Password.RequireLowercase = true;
         })
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();
        
        
        services.AddHangfire(config =>
         {
          config.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));
         }
         );
        services.AddHangfireServer();

       
       //repository 
       services.AddScoped<IUnitOfWork, UnitOfWork>();


        // connect vnpay
       services.AddScoped<IVnPayService, VnPayService>();

        //export file 
       services.AddScoped<IExporter, Exporter>();


        //intialDB 
       services.AddScoped<IDbInitializer, DbInitializer>();
       
       //email 
       services.AddScoped<IEmailService, EmailService>();
       
       //qrcode
       services.AddScoped<IQRCoderService, QRCoderService>();
       
       //systemTask
       // Recurring task
       services.AddScoped<IJobScheduler, JobScheduler>();
       //Conditional task
       services.AddScoped<IBackgroundJobScheduler, BackgroundJobScheduler>();
       
       
       //Sentiment rating
       var root = Directory.GetCurrentDirectory();
       var modelPath = Path.Combine(root, "AIServices", "SentimentModels","model.onnx");
       var vocabPath = Path.Combine(root, "AIServices", "SentimentModels", "vocab.txt");
       services.AddSingleton<IOnnxSentimentService>(new OnnxSentimentService(modelPath, vocabPath));

        return services;
    }
}