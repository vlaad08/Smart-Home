namespace WebAPI.DTOs;

public class RoomChangeDTO
{
    public string? Name { get; set; }
    public string? DeviceId { get; set; }
    public int? PreferedTemperature { get; set; }
    public int? PreferedHumidity { get; set; }
}