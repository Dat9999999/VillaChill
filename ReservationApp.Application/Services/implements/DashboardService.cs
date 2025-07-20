using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Services.implements;

public class DashboardService: IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
    private DateTime previousStartMonthDate = new DateTime(DateTime.Now.Year, previousMonth, 1);
    private DateTime currentStartMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public RadialBarChartDTO GetRadialCartDataModel(int total, double currentMonth, double prevMonth)
    {
        RadialBarChartDTO chart = new RadialBarChartDTO();
        int ratio = 100;
        if (prevMonth != 0)
        {
            ratio = Convert.ToInt32(currentMonth - prevMonth / prevMonth * 100);
        }
        chart.CountInCurrentMonth = Convert.ToInt32(currentMonth);
        chart.TotalCount = total;
        chart.IsIncrease = currentMonth > prevMonth;
        chart.series = new []{ratio};
        return chart;
    }

    public RadialBarChartDTO GetRevenueRadialChartData()
    {
        var totalBookings = _unitOfWork.Bookings.GetAll(u => u.Status != SD.StatusCancelled
                                                             && u.Status != SD.StatusPending);
        var totalRevenue = totalBookings.Sum(x => x.TotalCost);
        var totalRevenueByCurrMonth = totalBookings.Where(x => x.BookingDate >= currentStartMonthDate && x.BookingDate <= DateTime.Now)
            .Sum(x => x.TotalCost);
        var totalRevenueByPrevMonth= totalBookings.Where(x => x.BookingDate >= previousStartMonthDate && x.BookingDate < currentStartMonthDate)
            .Sum(x => x.TotalCost);
        return GetRadialCartDataModel(totalBookings.Count(), Convert.ToInt32(totalRevenueByCurrMonth),
            Convert.ToInt32(totalRevenueByPrevMonth));
    }

    public RadialBarChartDTO GetTotalBookingRadialChartData()
    {
        var totalBookings = _unitOfWork.Bookings.GetAll(u => u.Status != SD.StatusCancelled
                                                             && u.Status != SD.StatusPending);
        var totalBookingsByCurrMonth = totalBookings.Count(x => x.BookingDate >= currentStartMonthDate && x.BookingDate <= DateTime.Now);
        var totalBookingsByPrevMonth = totalBookings.Count(x => x.BookingDate >= previousStartMonthDate && x.BookingDate < currentStartMonthDate);
        return GetRadialCartDataModel(totalBookings.Count(), totalBookingsByCurrMonth, totalBookingsByPrevMonth);
    }

    public RadialBarChartDTO GetUserRegisteredRadialChartData()
    {
        var totalUser = _unitOfWork.ApplicationUsers.GetAll();
        var totalUserByCurrMonth = totalUser.Count(x => x.CreatedAt >= currentStartMonthDate && x.CreatedAt <= DateTime.Now);
        var totalUserByPrevMonth = totalUser.Count(x => x.CreatedAt >= previousStartMonthDate && x.CreatedAt < currentStartMonthDate);
        return GetRadialCartDataModel(totalUser.Count(), totalUserByCurrMonth, totalUserByPrevMonth);
    }

    public LineChartDTO getCustomerAndBookingLineChart()
    {
         var bookingData = _unitOfWork.Bookings.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
                                                          u.BookingDate.Date <= DateTime.Now)
            .GroupBy(b => b.BookingDate.Date)
            .Select(u => new {
                DateTime = u.Key,
                NewBookingCount = u.Count()
            });

        var customerData = _unitOfWork.ApplicationUsers.GetAll(u => u.CreatedAt >= DateTime.Now.AddDays(-30) &&
                                                        u.CreatedAt.Date <= DateTime.Now)
            .GroupBy(b => b.CreatedAt.Date)
            .Select(u => new {
                DateTime = u.Key,
                NewCustomerCount = u.Count()
            });
        var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
            (booking, customer) => new
            {
                booking.DateTime,
                booking.NewBookingCount,
                NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault()
            });


        var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
            (customer, booking) => new
            {
                customer.DateTime,
                NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
                customer.NewCustomerCount
            });

        var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

        var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
        var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
        var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

        List<LineChartData> chartDataList = new()
        {
            new LineChartData()
            {
                name = "New Bookings",
                data = newBookingData
            },
            new LineChartData()
            {
                name = "New Members",
                data = newCustomerData
            },
        };

        LineChartDTO LineChartDto = new()
        {
            categories = categories,
            series = chartDataList
        };

        return LineChartDto;
    }

    public PieChartDTO GetBookingPieChartData()
    {
        var totalBookingsIn30Days = _unitOfWork.Bookings.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&(u.Status != SD.StatusCancelled
            && u.Status != SD.StatusPending), "User");
        var CustomerWithOneBooking = totalBookingsIn30Days.GroupBy(b => b.User.Id).Where(x => x.Count() == 1).Select(x => x.Key).ToList();
        var NewCustomer = CustomerWithOneBooking.Count();
        var ReturnCustomer = totalBookingsIn30Days.Count() - NewCustomer;
        PieChartDTO pieChart = new PieChartDTO()
        {
            labels = new []{"New Customer", "Return Customer"},
            series = new []{NewCustomer, ReturnCustomer}
        };
        return pieChart;
    }

    public PieChartDTO GetVillaBookingPieChart(string ownerEmail)
    {
        var data = _unitOfWork.Bookings
            .GetAll(u => u.Villa.OwnerEmail == ownerEmail && u.Status == SD.StatusCompleted, "Villa")
            .GroupBy(b => b.Villa.Name)
            .Select(x => new {
                villaName = x.Key,
                bookingCount = x.Count()
            });;
        PieChartDTO pieChart = new PieChartDTO()
        {
            labels = data.Select(x => x.villaName).ToArray(),
            series = data.Select(x => x.bookingCount).ToArray()
        };
        return pieChart;
    }

    public int GetGetNumberOfVilla(string ownerEmail)
    {
        var data = _unitOfWork.Villas.GetAll(u => u.OwnerEmail == ownerEmail).Count();
        return data;       
    }
}