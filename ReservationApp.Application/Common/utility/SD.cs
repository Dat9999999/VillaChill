using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.utility;

public static class SD
{
    // user role 
    public const string Role_Customer = "Customer";
    public const string Role_Admin = "Admin";
    public const string Role_Owner = "Owner";
    
    // booking status
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusCheckedIn = "CheckedIn";
    public const string StatusCompleted = "Completed";
    public const string StatusCancelled = "Cancelled";
    public const string StatusRefunded = "Refunded";
    
    public const double UsdDiffVND = 25000;
    
    // email 
    public const string SenderName = "VillaChill";
    public const string bookingSuccessEmailTitle  = "Your booking is successful";
    public const string bookingSuccessEmailBody = "You got this email because you have just paid for booking Id: {0} and your room number is: {1} " +
                                                  "if anything wrong please contact us at " + SenderName;
    
    public const string adminMail  = "huynhtandat184@gmail.com";
    public const string ownerMail  = "dathuynhfinance@gmail.com";
    public const string customerMail  = "datrootx@gmail.com";
    
    
    //document string
    public const string ThanksMessage = "Thank you for choosing our Villa Service!";
    public const string LogoPath = "wwwroot/images/resort.png";
    public const string InvoiceTitle = "VILLA BOOKING INVOICE";
    public const string RevenueTitle = "Revenue Report";
    
    public const string reportHtml = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        body {
            font-family: Arial, sans-serif;
            color: #333;
            background-color: #f8f9fa;
            padding: 20px;
        }
        .container {
            background-color: #ffffff;
            border-radius: 8px;
            padding: 30px;
            max-width: 600px;
            margin: auto;
            box-shadow: 0 0 10px rgba(0,0,0,0.05);
        }
        h2 {
            color: #007bff;
        }
        .section-title {
            margin-top: 30px;
            margin-bottom: 10px;
            font-weight: bold;
        }
        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }
        th, td {
            padding: 10px 12px;
            border: 1px solid #dee2e6;
            text-align: left;
        }
        th {
            background-color: #f1f3f5;
        }
        .footer {
            margin-top: 30px;
            font-size: 13px;
            color: #888;
            text-align: center;
        }
    </style>
</head>
<body>
<div class=""container"">
    <h2>📊 Weekly Revenue Report</h2>
    <p>Hi <strong>{OwnerName}</strong>,</p>
    <p>Here is your revenue report for the past week:</p>

    <div class=""section-title"">Summary</div>
    <table>
        <tr>
            <th>Total Revenue</th>
            <td>{TotalRevenue}</td>
        </tr>
        <tr>
            <th>Number of Bookings</th>
            <td>{NumberBookings}</td>
        </tr>
    </table>

    <div class=""section-title"">Daily Breakdown</div>
    <table>
        <tr>
            <th>Date</th>
            <th>Revenue</th>
        </tr>
        {DailyRevenueRows}
    </table>

    <p>We've also attached a detailed invoice for your records.</p>

    <p>Thank you for choosing our platform!</p>

    <div class=""footer"">
        &copy; {Year} VillaChill Platform. All rights reserved.
    </div>
</div>
</body>
</html>";
    
    //payment Method 
    public const string PaymentMethod_Online = "Online";
    public const string PaymentMethod_Onsite = "Onsite";
    
    // status payment
    
    public const string StatusPayment_Unpaid = "Unpaid";
    public const string StatusPayment_Paid = "Paid";
    public const string StatusPayment_Overdue = "Overdue";
    
    // duedate = paid date + 30 days
    public const int DueDate = 30;
    
    
    //commission rate name 
    public const string CommissionRate_platform = "Platform fee";
    public static HashSet<int> VillaRoomsAvailable_Count(
        int villaId, 
        List<VillaNumber> villaNumberList, 
        DateOnly checkInDate, 
        int nights,
        List<Booking> bookings)
    {
        // Lấy toàn bộ danh sách phòng của villa này
        var allRoomNumbers = villaNumberList
            .Where(x => x.VillaId == villaId)
            .Select(x => x.Villa_Number)
            .ToList();

        List<HashSet<int>> availableEachNight = new();

        for (int i = 0; i < nights; i++)
        {
            var date = checkInDate.AddDays(i);

            // Danh sách phòng đã đặt trong đêm này
            var bookedRooms = bookings
                .Where(b => b.VillaId == villaId &&
                            b.CheckInDate <= date &&
                            b.CheckOutDate > date)
                .Select(b => b.VillaNumber)
                .ToHashSet();

            // Lấy phòng còn trống trong đêm này
            var availableRoomsTonight = allRoomNumbers
                .Where(room => !bookedRooms.Contains(room))
                .ToHashSet();

            availableEachNight.Add(availableRoomsTonight);
        }

        // Tìm phòng trống trong toàn bộ nights (giao nhau)
        var finalAvailable = availableEachNight
            .Aggregate((set1, set2) => set1.Intersect(set2).ToHashSet());

        return finalAvailable;
    }
}