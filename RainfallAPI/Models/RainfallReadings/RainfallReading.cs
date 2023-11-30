namespace RainfallAPI.Models.RainfallReadings;

public class RainfallReading
{
    public string? Id { get; set; }
    public DateTime DateMeasured { get; set; }
    public double AmountMeasured { get; set; }
}
