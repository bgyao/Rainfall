namespace RainfallAPI.Models.Readings.ResponseHandler.Components.Metas;

public class Meta
{
    public string Publisher { get; set; }
    public string Licence { get; set; }
    public string Documentation { get; set; }
    public string Version { get; set; }
    public string Comment { get; set; }
    public List<MetaFormat> HasFormat { get; set; }
    public int Limit { get; set; }
}
