using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ReservationApp.Domain.Entities;

namespace ReservationApp.ViewModels;

public class AmenitiesVM
{
    public Amenity Amenity { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> villas { get; set; }
}