using Microsoft.AspNetCore.Identity;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;

namespace ReservationApp.Infrastructure.Email;
using System.Net;
using System.Net.Mail;


public class EmailService : IEmailService
{
    public void SendEmail(string receiverEmail, string subject, string body)
    {
        var fromAddress = new MailAddress("huynhtandat184@gmail.com", SD.SenderName);
        var toAddress = new MailAddress(receiverEmail, receiverEmail);
        const string fromPassword = "cbum qlay mbhq iodo";

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com", // hoặc SMTP server khác như mail server của công ty
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
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
