using System.ComponentModel.DataAnnotations;

namespace ReservationApp.ViewModels;

public class OwnerSettlementDTO
{
    [Required]
    public int BookingId { get; set; }
    [Required]
    public string OwnerId { get; set; }
    [Required]
    public double Amount { get; set; } // 👉 Tổng tiền Owner đã nhận
    public double? CommissionRate { get; set; } // Ví dụ: 15.00 nghĩa là 15%

    [Required]
    public string Status { get; set; } // "Unpaid", "Paid", "Overdue"

    [Required]
    public DateTime DueDate { get; set; }
}