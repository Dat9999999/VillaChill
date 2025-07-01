using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Services.implements;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;
using ReservationApp.Infrastructure.Data;
using ReservationApp.Infrastructure.Exporting;
using ReservationApp.Infrastructure.Payments;
using ReservationApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// connect vnpay
builder.Services.AddScoped<IVnPayService, VnPayService>();

//export file 
builder.Services.AddScoped<IExporter, Exporter>();


//intialDB 
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

//services 
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IVillaService, VillaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();
SeedData();
app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();


//seed data when the server runs for the first time
void SeedData()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInit = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInit.Initialize();
    }
}