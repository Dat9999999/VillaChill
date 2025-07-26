using QRCoder;
using ReservationApp.Application.Common.Interfaces;

public class QRCoderService : IQRCoderService
{

    public byte[] GenerateQRCode(string data)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            byte[] qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }
    }
}