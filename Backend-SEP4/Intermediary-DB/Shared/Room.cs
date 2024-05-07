using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DBComm.Shared;

public class Room
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string DeviceId { get; set; }
    public Home Home { get; set; }
    [JsonIgnore]
    public ICollection<HumidityReading> HumidityReadings { get; set; }
    [JsonIgnore]
    public ICollection<TemperatureReading> TemperatureReadings { get; set; }
    [JsonIgnore]
    public ICollection<LightReading> LightReadings { get; set; }

    public Room()
    {
        
    }
    
}