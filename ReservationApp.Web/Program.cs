using Hangfire;
using ReservationApp.Application;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Hubs;
using ReservationApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// controller with view
builder.Services.AddControllersWithViews();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials() 
            .SetIsOriginAllowed(_ => true); 
    });
});
//SignalR
builder.Services.AddSignalR();

//Dependency injection
builder.Services.
    AddApplication()
    .AddInfrastructure(builder.Configuration);

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
// CORS
app.UseCors();

app.UseAuthorization();

SeedData();
app.MapStaticAssets();


//SignalR
app.MapHub<DashBoardHub>("/dashboardHub");

//hangfire 
app.UseHangfireDashboard();
ScheduleRecurringJob();


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

void ScheduleRecurringJob()
{
    using (var scope = app.Services.CreateScope())
    {
        var jobScheduler = scope.ServiceProvider.GetRequiredService<IJobScheduler>();
        jobScheduler.ScheduleJob();       
    }
}