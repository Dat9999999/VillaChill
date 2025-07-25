﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ReservationApp.Domain.Entities;

public class Villa
{
    public int Id { get; set; }
    [MaxLength(50, ErrorMessage = "Name cannot be more than 50 characters")]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Range(100, 1000000, ErrorMessage = "Square footage must be between 100 and 1000000")]
    public int Sqft { get; set; }
    [Range(1, 10, ErrorMessage = "Occupancy must be between 1 and 10")]
    public int Occupancy { get; set; }
    [DisplayName( "Price per night")]
    [Range(100000, 1000000000, ErrorMessage = "Price must be between 100,000 and 1,000,000,000")]
    public double Price { get; set; }
    [NotMapped]
    public IFormFile? Image { get; set; }
    [DisplayName( "Image Url")]
    public string? ImageUrl { get; set; }
    
    public string? Address { get; set; }
    public string? City { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [Required]
    public string OwnerEmail { get; set; }
    
    [ValidateNever]
    public IEnumerable<Amenity> Amenities { get; set; }
    
    [NotMapped]
    public bool IsAvaliable { get; set; } = true;
    
    [ValidateNever]
    public IEnumerable<Rating> Ratings { get; set; }
}