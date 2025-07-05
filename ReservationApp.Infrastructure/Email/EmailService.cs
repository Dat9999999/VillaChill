using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;

namespace ReservationApp.Infrastructure.Email;
using System.Net;
using System.Net.Mail;


public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void SendEmail(string receiverEmail, string subject, string body)
    {
        var senderEmail = _configuration["email:senderEmail"];
        var senderPassword = _configuration["email:password"];
        
        var fromAddress = new MailAddress(senderEmail, SD.SenderName);
        var toAddress = new MailAddress(receiverEmail, receiverEmail);

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(fromAddress.Address, senderPassword),
            Timeout = 20000
        };

        using (var message = new MailMessage(fromAddress, toAddress)
               {
                   Subject = subject,
                   Body = body
               })
        {
            smtp.Send(message);
        }
    }

    public void configMailPaySuccess(string receiverEmail, string villaName, int villaNumbers)
    {
        string body =  string.Format(SD.bookingSuccessEmailBody, villaName, villaNumbers);
        string subject = SD.bookingSuccessEmailTitle;
        SendEmail(receiverEmail, subject, body);
    }
}
