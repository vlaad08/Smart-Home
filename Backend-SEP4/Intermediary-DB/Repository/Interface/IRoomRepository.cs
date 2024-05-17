using DBComm.Shared;
using WebAPI.DTOs;

namespace DBComm.Repository;

public interface IRoomRepository
{
    Task AddRoom(string name, string deviceId, string homeId, int preferedTemperature, int preferedHumidity);
    Task DeleteRoom(string id);
    Task EditRoom(string id,string? name, string? deviceId, int? preferedTemperature, int? preferedHumidity);
    Task<List<Room>?> GetAllRooms(string homeId);
    Task<RoomDataTransferDTO> GetRoomData(string homeId, string? deviceId,bool temp=false, bool humi=false, bool light=false);

    Task<bool> CheckExistingRoom(string deviceId, string homeId);
    Task<bool> CheckNonExistingRoom(string deviceId);
    Task SetRadiatorLevel(string deviceId, int level);
    Task<int> GetRadiatorLevel(string deviceId);
    Task SaveWindowState(string houseId, bool state);
    Task<bool> GetWindowState(string houseId);
    Task SetLightState(string hardwareId, int level);
    Task<int> GetLightState(string hardwareId);
}