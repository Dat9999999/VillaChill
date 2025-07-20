using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.interfaces;

public interface IDashboardService
{
    public RadialBarChartDTO GetRadialCartDataModel(int total, double currentMonth, double prevMonth);
    public RadialBarChartDTO GetRevenueRadialChartData();
    public RadialBarChartDTO GetTotalBookingRadialChartData();
    public RadialBarChartDTO GetUserRegisteredRadialChartData();
    public LineChartDTO getCustomerAndBookingLineChart();
    public PieChartDTO GetBookingPieChartData();

    public PieChartDTO GetVillaBookingPieChart(string ownerEmail);
    public int GetGetNumberOfVilla(string ownerEmail);
}