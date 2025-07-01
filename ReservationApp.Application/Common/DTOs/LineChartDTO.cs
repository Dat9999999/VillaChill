namespace ReservationApp.ViewModels;

public class LineChartDTO
{
    public List<LineChartData> series { get; set; }
    public string[] categories { get; set; }
}
public class LineChartData
{
    public string name { get; set; }
    public int[] data { get; set; }
}