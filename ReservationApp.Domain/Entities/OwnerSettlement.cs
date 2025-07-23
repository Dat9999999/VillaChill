using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ReservationApp.Domain.Entities
{
    /// <summary>
    /// Represents the commission/settlement that an Owner must pay Admin for an onsite booking.
    /// </summary>
    public class OwnerSettlement
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public int BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        [ValidateNever]
        public Booking Booking { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        [ValidateNever]
        public ApplicationUser Owner { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // 👉 Tổng tiền Owner đã nhận

        [Column(TypeName = "decimal(5,2)")]
        public decimal? CommissionRate { get; set; } // Ví dụ: 15.00 nghĩa là 15%

        public string? PaymentMethod { get; set; } // "Cash", "Bank Transfer", v.v.
        public string? PaymentId { get; set; }     // Mã giao dịch nếu có

        [Required]
        public string Status { get; set; } // "Unpaid", "Paid", "Overdue"

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}