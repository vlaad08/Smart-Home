using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class HumidityReading
{
    [Key]
    public string Id { get; set; }
    public double Value { get; set; }
    public DateTime ReadAt { get; set; }
    public Room? Room { get; set; }

    public HumidityReading()
    {
        
    }

    public HumidityReading(double value, DateTime readAt)
    {
        Id = Guid.NewGuid().ToString();
        Value = value;
        ReadAt = readAt;
    }
}