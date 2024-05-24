namespace WebAPI.DTOs;

public class RoomCreationDTO
{
    public string deviceId { get; set; }
    public string homeId { get; set; }
    public string name { get; set; }
    public int PreferedTemperature { get; set; }
    public int PreferedHumidity { get; set; }
}