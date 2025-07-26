namespace ReservationApp.Application.Common.Interfaces;

public interface IQRCoderService
{
    public byte[] GenerateQRCode(string data);
}