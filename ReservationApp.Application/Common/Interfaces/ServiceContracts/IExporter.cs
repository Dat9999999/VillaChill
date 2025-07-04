using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IExporter
{
    byte[] ExportBookingInvoice(Booking booking);
}