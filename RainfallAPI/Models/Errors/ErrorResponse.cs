namespace RainfallAPI.Models.Errors;

public class ErrorResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public Detail Detail { get; set; }
}
