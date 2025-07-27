using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;

namespace ReservationApp.Application.Common.Interfaces;

public interface IExporter
{
    byte[] ExportBookingInvoice(Booking booking);
    byte[] ExportRevenueReport(RevenueReportDto revenueReportDto);
}