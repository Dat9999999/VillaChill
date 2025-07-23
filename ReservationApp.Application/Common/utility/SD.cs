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
    
    
    //invoice string
    public const string ThanksMessage = "Thank you for choosing our Villa Service!";
    public const string LogoPath = "wwwroot/images/resort.png";
    public const string InvoiceTitle = "VILLA BOOKING INVOICE";
    
    
    //payment Method 
    public const string PaymentMethod_Online = "Online";
    public const string PaymentMethod_Onsite = "Onsite";
    
    
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