﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DBComm.Shared;

public class Room
{
    [Key]
    public string Id { get; set; }
    public string Name { get; set; }
    public string DeviceId { get; set; }
    public Home Home { get; set; }
    public bool IsWindowOpen{ get; set;} 
    public int LightLevel { get; set; }
    public int RadiatorState { get; set; }
    public int? PreferedTemperature { get; set; }
    public int? PreferedHumidity { get; set; }
    [JsonIgnore]
    public ICollection<HumidityReading> HumidityReadings { get; set; }
    [JsonIgnore]
    public ICollection<TemperatureReading> TemperatureReadings { get; set; }
    [JsonIgnore]
    public ICollection<LightReading> LightReadings { get; set; }

    public Room(string name, string deviceId)
    {
        this.Id = Guid.NewGuid().ToString();
        this.Name = name;
        this.DeviceId = deviceId;
    }
    
}