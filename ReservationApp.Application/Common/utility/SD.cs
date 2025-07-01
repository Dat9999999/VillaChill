using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.utility;

public static class SD
{
    public const string Role_Customer = "Customer";
    public const string Role_Admin = "Admin";
    
    public const string StatusPending = "Pending";
    public const string StatusApproved = "Approved";
    public const string StatusCheckedIn = "CheckedIn";
    public const string StatusCompleted = "Completed";
    public const string StatusCancelled = "Cancelled";
    public const string StatusRefunded = "Refunded";
    public const double UsdDiffVND = 25000;
    
    
    public const string ThanksMessage = "\nThank you for choosing our Villa Service!";
    
    public static int VillaRoomsAvailable_Count(int villaId, 
        List<VillaNumber> villaNumberList, DateOnly checkInDate, int nights,
        List<Booking> bookings)
    {
        HashSet<int> bookingInDate = new();
        int finalAvailableRoomForAllNights = int.MaxValue;
        var roomsInVilla = villaNumberList.Where(x => x.VillaId == villaId).Count();

        for(int i = 0; i < nights; i++)
        {
            var villasBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i)
                                                   && u.CheckOutDate > checkInDate.AddDays(i) && u.VillaId == villaId);

            foreach(var booking in villasBooked)
            {
                if (!bookingInDate.Contains(booking.Id))
                {
                    bookingInDate.Add(booking.Id);
                }
            }

            var totalAvailableRooms = roomsInVilla - bookingInDate.Count;
            if(totalAvailableRooms == 0)
            {
                return 0;
            }
            if(finalAvailableRoomForAllNights > totalAvailableRooms)
            {
                finalAvailableRoomForAllNights = totalAvailableRooms;
            }
            
        }

        return finalAvailableRoomForAllNights;
    }
}