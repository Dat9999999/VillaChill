using System.ComponentModel.DataAnnotations;

namespace ReservationApp.ViewModels;

public class RegisterVM
{
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [Display(Name = "Confirm password")]
    public string? ConfirmPassword { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    public string? ReturnUrl { get; set; }
}