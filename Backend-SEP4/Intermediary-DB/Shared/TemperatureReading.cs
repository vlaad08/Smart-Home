using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class TemperatureReading
{
    [Key]
    public string Id { get; set; }
    public double Value { get; set; }
    public DateTime ReadAt { get; set; }
    public Room? Room { get; set; }

    public TemperatureReading()
    {
        
    }

    public TemperatureReading(double value, DateTime readAt)
    {
        Id = Guid.NewGuid().ToString();
        Value = value;
        ReadAt = readAt;
    }
}