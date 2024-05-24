using DBComm.Shared;
using WebAPI.DTOs;

namespace DBComm.Logic.Interfaces;

public interface IRoomLogic
{
    Task AddRoom(string name, string deviceId, string homeId, int preferedTemperature, int preferedHumidity);
    Task DeleteRoom(string deviceId);
    Task EditRoom(string id,string? name, string? deviceId, int? preferedTemperature, int? preferedHumidity);
    Task<List<RoomDataDTO>?> GetAllRooms(string homeId);
    Task SetRadiatorLevel(string deviceId, int level);
    Task<int> GetRadiatorLevel(string deviceId);
    Task SaveWindowState(string hardwareId, bool state);
    Task<bool> GetWindowState(string hardwareId);
    Task SetLightState(string hardwareId, int level);
    Task<int> GetLightState(string hardwareId);
    Task<RoomDataDTO> GetRoomData(string homeId, string deviceId, bool temp=false, bool humi=false, bool light=false);
}