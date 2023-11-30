namespace RainfallAPI.Models.Errors;

public class Detail
{
    public Guid Id { get; set; }
    public List<ErrorDetail> Items { get; set; }
}
