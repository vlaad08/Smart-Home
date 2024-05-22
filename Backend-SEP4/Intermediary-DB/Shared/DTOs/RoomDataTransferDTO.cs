using DBComm.Shared;

namespace WebAPI.DTOs;

public class RoomDataTransferDTO
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? DeviceId { get; set; }
    public Home? Home { get; set; }
    public bool? IsWindowOpen{ get; set;} 
    public int? LightLevel { get; set; }
    public int? RadiatorState { get; set; }
    public int? PreferedTemperature { get; set; }
    public int? PreferedHumidity { get; set; }
    public double? TempValue { get; set; }
    public double? HumiValue { get; set; }
    public double? LightValue { get; set; }
}