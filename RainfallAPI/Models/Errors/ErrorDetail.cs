namespace RainfallAPI.Models.Errors;

public class ErrorDetail
{
    public Guid Id { get; set; }
    public string PropertyName { get; set; }
    public string Message { get; set; }

}
