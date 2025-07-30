namespace ReservationApp.Views.ViewHelper;

public static class ViewFunction
{
    public static string GetAmenityIconClass(string name)
    {
        name = name.ToLower();
        return name switch
        {
            var n when n.Contains("wifi") => "bi bi-wifi",
            var n when n.Contains("pool") => "bi bi-water",
            var n when n.Contains("microwave") => "bi bi-lightning",
            var n when n.Contains("balcony") => "bi bi-columns",
            var n when n.Contains("sofa") || n.Contains("bed") => "bi bi-bed",
            var n when n.Contains("tv") => "bi bi-tv",
            var n when n.Contains("air") => "bi bi-thermometer",
            var n when n.Contains("kitchen") => "bi bi-house-door",
            var n when n.Contains("parking") => "bi bi-car-front",
            _ => "bi bi-check-circle"
        };
    }
}