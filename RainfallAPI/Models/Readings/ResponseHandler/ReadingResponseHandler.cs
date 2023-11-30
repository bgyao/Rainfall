using RainfallAPI.Models.RainfallReadings;
using RainfallAPI.Models.Readings.ResponseHandler.Components.Metas;

namespace RainfallAPI.Models.Readings.ResponseHandler;

public class ReadingResponseHandler
{
    public string Context { get; set; }
    public Meta Meta { get; set; }
    public List<RainfallReadingFromSource> Items { get; set; }

}
