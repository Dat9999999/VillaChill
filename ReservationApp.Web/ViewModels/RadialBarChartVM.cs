namespace ReservationApp.ViewModels;

public class RadialBarChartVM
{
    public decimal TotalCount { get; set; }
    public decimal CountInCurrentMonth { get; set; }
    public bool IsIncrease { get; set; }
    public int[] series { get; set; }
}