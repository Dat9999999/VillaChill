using System.Data;
using System.Linq.Expressions;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Application.Services.interfaces;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Services.implements;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBackgroundJobScheduler _backgroundJobScheduler;
    private readonly ICacheService _cacheService;
    public BookingService(IUnitOfWork unitOfWork, 
        IBackgroundJobScheduler backgroundJobScheduler,
        ICacheService cacheService)
    {
        _unitOfWork = unitOfWork;
        _backgroundJobScheduler = backgroundJobScheduler;      
        _cacheService = cacheService;      
    }
    public IEnumerable<Booking> GetAll(Expression<Func<Booking, bool>>? filter = null,string includeProperties = "")
    {
        return _unitOfWork.Bookings.GetAll(filter, includeProperties);
    }

    public Booking GetById(Expression<Func<Booking, bool>>? filter = null, string includeProperties = "")
    {
        return _unitOfWork.Bookings.Get(filter, includeProperties);       
    }

    public void UpdateStatus(int bookingId, string status)
    {
        _unitOfWork.Bookings.UpdateStatus(bookingId, status);
        _unitOfWork.Save();
    }

    public void UpdatePaymentId(int bookingId, string PaymentId)
    {
        _unitOfWork.Bookings.UpdatePaymentId(bookingId, PaymentId);
        _unitOfWork.Save();
    }

    public void Add(Booking booking, string paymentMethod)
    {

        using var transaction = _unitOfWork.BeginTransaction(IsolationLevel.Serializable);
        try
        {
            
            // 1. check available room in villa
            var overlappingBooking = _unitOfWork.Bookings.Any(b =>
                b.VillaId == booking.VillaId && booking.VillaNumber == b.VillaNumber &&
                (
                    (booking.CheckInDate >= b.CheckInDate && booking.CheckInDate < b.CheckOutDate) ||
                    (booking.CheckOutDate > b.CheckInDate && booking.CheckOutDate <= b.CheckOutDate) ||
                    (booking.CheckInDate <= b.CheckInDate && booking.CheckOutDate >= b.CheckOutDate)
                )
            );

            if (overlappingBooking)
            {
                throw new Exception("Room is already booked. Please try another room or villa.");
            }
            // 2. Add booking
            var villa = _unitOfWork.Villas.Get(x => x.Id == booking.VillaId);;
            booking.TotalCost = villa.Price * booking.Nights;
        
            booking.Status = SD.StatusPending;
            booking.BookingDate = DateTime.Now;
            booking.IsPaidAtCheckIn = paymentMethod == SD.PaymentMethod_Onsite;

            _unitOfWork.Bookings.Add(booking);

            

            // 3. Save and commit
            _unitOfWork.Save();
            transaction.Commit();       
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
        //count down for 3 mins to complete payment 
        if (paymentMethod == SD.PaymentMethod_Online)
        {
            var delay = TimeSpan.FromMinutes(3);
            _backgroundJobScheduler.ScheduleCancelBooking(booking.Id, delay);
        }
    }

    public void CreateCheckInToken(int bookingId)
    {
        var booking = _unitOfWork.Bookings.Get(x => x.Id == bookingId);
        booking.CheckInToken = Guid.NewGuid().ToString();
        _unitOfWork.Save();       
    }

    public Booking CheckQRCodeVallid(int bookingId, string checkinToken)
    {
        var booking = _unitOfWork.Bookings.Get(x => x.Id == bookingId && x.CheckInToken == checkinToken);
        return booking;       
    }

    public void CancelBooking(int bookingId)
    {
        var booking = _unitOfWork.Bookings.Get(x => x.Id == bookingId);
        if(booking.IsPaymentSuccessful) return;
        booking.Status = SD.StatusCancelled;
        _unitOfWork.Bookings.Update(booking);       
        _unitOfWork.Save();       
    }

    public IEnumerable<Villa> CheckAvailability(int nights, DateOnly checkInDate, string city,int page, int pageSize)
    {
        var villaList = _cacheService.GetVillas(city, includeProperties: "Amenities", false, page, pageSize);
        var villasBooked = _unitOfWork.Bookings.GetAll(u => u.Status != SD.StatusCancelled).ToList();
        var villaNumbers = _unitOfWork.VillaNumbers.GetAll().ToList();
        foreach (var villa in villaList)
        {
            HashSet<int> roomAvailable = SD.VillaRoomsAvailable_Count(villa.Id, villaNumbers ,checkInDate, nights, villasBooked);
            villa.IsAvaliable = roomAvailable.Count > 0;
        }
        return villaList;       
    }
}