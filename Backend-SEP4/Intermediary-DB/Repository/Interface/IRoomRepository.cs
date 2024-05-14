using DBComm.Shared;
using WebAPI.DTOs;

namespace DBComm.Repository;

public interface IRoomRepository
{
    Task AddRoom(string name, string deviceId, string homeId);
    Task DeleteRoom(string id);
    Task EditRoom(string id,string? name=null, string? deviceId=null);
    Task<List<Room>?> GetAllRooms(string homeId, string? deviceId=null);
    Task<RoomDataTransferDTO> GetRoomData(string homeId, string? deviceId,bool temp=false, bool humi=false, bool light=false);

    Task<bool> CheckExistingRoom(string deviceId, string homeId);
    Task<bool> CheckNonExistingRoom(string id);
}