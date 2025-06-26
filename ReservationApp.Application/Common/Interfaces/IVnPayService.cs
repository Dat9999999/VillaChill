using Microsoft.AspNetCore.Http;
using ReservationApp.Domain.Entities;

namespace ReservationApp.Application.Common.Interfaces;

public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);

}