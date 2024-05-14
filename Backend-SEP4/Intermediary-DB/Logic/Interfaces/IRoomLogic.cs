using DBComm.Shared;
using WebAPI.DTOs;

namespace DBComm.Logic.Interfaces;

public interface IRoomLogic
{
    Task AddRoom(string name, string deviceId, string homeId);
    Task DeleteRoom(string id);
    Task EditRoom(string id,string? name=null, string? deviceId=null);
    Task<List<Room>?> GetAllRooms(string homeId, string? deviceId=null);

    Task<RoomDataTransferDTO> GetRoomData(string homeId, string deviceId, bool temp=false,bool humi=false,bool light=false);
}