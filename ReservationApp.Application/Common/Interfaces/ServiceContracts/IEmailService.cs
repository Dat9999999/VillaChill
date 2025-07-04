namespace ReservationApp.Application.Common.Interfaces;

public interface IEmailService
{
    public void SendEmail(string receiverEmail, string subject, string message);
    public void configMailPaySuccess(string receiverEmail, string villaName, int villaNumbers);
}