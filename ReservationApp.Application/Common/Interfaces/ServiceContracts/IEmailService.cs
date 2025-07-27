namespace ReservationApp.Application.Common.Interfaces;

public interface IEmailService
{
    public void SendEmail(string receiverEmail, string subject, string message, byte[] attachment = null);
    public void configMailPaySuccess(string receiverEmail, string villaName, int villaNumbers);
}