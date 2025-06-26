using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Villa>().HasData(
            new Villa
            {
                Id = 1,
                Name = "Royal Villa",
                Description = "A luxurious villa with stunning ocean views",
                Sqft = 2000,
                Occupancy = 4,
                Price = 500,
                ImageUrl = "/images/villa1.jpg"
            },
            new Villa
            {
                Id = 2,
                Name = "Sunset Villa",
                Description = "Cozy villa with a breathtaking sunset view",
                Sqft = 1500,
                Occupancy = 3,
                Price = 350,
                ImageUrl = "/images/villa2.jpg"
                
            },
            new Villa
            {
                Id = 3,
                Name = "Mountain Retreat",
                Description = "Secluded villa nestled in the mountains",
                Sqft = 1800,
                Occupancy = 4,
                Price = 400,
                ImageUrl = "/images/villa3.jpg"
            }
        );
        modelBuilder.Entity<VillaNumber>().HasData(
            new VillaNumber
            {
                Villa_Number = 101,
                VillaId = 1,
            },
            new VillaNumber
            {
                Villa_Number = 102,
                VillaId = 2,
            },new VillaNumber
            {
                Villa_Number = 103,
                VillaId = 3,
            },new VillaNumber
            {
                Villa_Number = 201,
                VillaId = 1,
            },new VillaNumber
            {
                Villa_Number = 202,
                VillaId = 2,
            },new VillaNumber
            {
                Villa_Number = 203,
                VillaId = 3,
            }
            );
        modelBuilder.Entity<Amenity>().HasData(
            new Amenity
            {
                Id = 1,
                VillaId = 1,
                Name = "Private Pool"
            }, new Amenity
            {
                Id = 2,
                VillaId = 1,
                Name = "Microwave"
            }, new Amenity
            {
                Id = 3,
                VillaId = 1,
                Name = "Private Balcony"
            }, new Amenity
            {
                Id = 4,
                VillaId = 1,
                Name = "1 king bed and 1 sofa bed"
            },

            new Amenity
            {
                Id = 5,
                VillaId = 2,
                Name = "Private Plunge Pool"
            }, new Amenity
            {
                Id = 6,
                VillaId = 2,
                Name = "Microwave and Mini Refrigerator"
            }, new Amenity
            {
                Id = 7,
                VillaId = 2,
                Name = "Private Balcony"
            }, new Amenity
            {
                Id = 8,
                VillaId = 2,
                Name = "king bed or 2 double beds"
            },

            new Amenity
            {
                Id = 9,
                VillaId = 3,
                Name = "Private Pool"
            }, new Amenity
            {
                Id = 10,
                VillaId = 3,
                Name = "Jacuzzi"
            }, new Amenity
            {
                Id = 11,
                VillaId = 3,
                Name = "Private Balcony"
            });
    }
}