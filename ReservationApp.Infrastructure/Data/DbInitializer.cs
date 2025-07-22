using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Infrastructure.Data;

public class DbInitializer : IDbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, 
        RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public void Initialize()
{
    try
    {
        if (_context.Database.GetPendingMigrations().Any())
        {
            _context.Database.Migrate();
        }

        // Tạo các role nếu chưa có
        if (!_roleManager.RoleExistsAsync(SD.Role_Admin).Result)
        {
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Owner)).Wait();
            _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
        }

        // Tạo tài khoản Admin
        if (_userManager.FindByEmailAsync(SD.adminMail).Result == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = SD.adminMail,
                Email = SD.adminMail,
                EmailConfirmed = true,
                Name = "Admin"
            };

            var result = _userManager.CreateAsync(adminUser, "Admin123@").Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(adminUser, SD.Role_Admin).Wait();
            }
        }

        // Tạo tài khoản Owner
        if (_userManager.FindByEmailAsync(SD.ownerMail).Result == null)
        {
            var ownerUser = new ApplicationUser
            {
                UserName = SD.ownerMail,
                Email = SD.ownerMail,
                EmailConfirmed = true,
                Name = "Owner One"
            };

            var result = _userManager.CreateAsync(ownerUser, "Owner123@").Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(ownerUser, SD.Role_Owner).Wait();
            }
        }

        // Tạo tài khoản Customer
        if (_userManager.FindByEmailAsync(SD.customerMail).Result == null)
        {
            var customerUser = new ApplicationUser
            {
                UserName = SD.customerMail,
                Email = SD.customerMail,
                EmailConfirmed = true,
                Name = "Customer One"
            };

            var result = _userManager.CreateAsync(customerUser, "Customer123@").Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(customerUser, SD.Role_Customer).Wait();
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

}