namespace ReservationApp.Application.Services.interfaces;

public interface IOnnxSentimentService
{
    public string Predict(string text);
}