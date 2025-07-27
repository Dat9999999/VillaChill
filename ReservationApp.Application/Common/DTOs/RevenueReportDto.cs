using System.Runtime.InteropServices.JavaScript;

namespace ReservationApp.ViewModels;

public class RevenueReportDto
{
    public double totalRevenue { get; set; }
    public IEnumerable<DailyRevenueDto> revenueByWeek{ get; set; }
    public int numberBookings{ get; set; }
}