using System.ComponentModel.DataAnnotations;

namespace RainfallAPI.Models.Readings;

public class Reading
{
    public int Id { get; set; }

    //<summary>
    //The id of the Reading Station
    //</summary>
    [Required]
    public string StationId { get; set; }

    //<summary>
    //The number of readings to return
    //</summary>
    [Range(1, 100, ErrorMessage ="Please enter a value from 1 to 100")]
    public int Count { get; set; } //default value is 10

    public Reading(string stationId, byte count = 10)
    {
        StationId = stationId;
        Count = count;
    }
}
