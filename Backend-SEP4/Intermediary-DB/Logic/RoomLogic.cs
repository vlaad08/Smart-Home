using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using WebAPI.DTOs;

namespace DBComm.Logic;

public class RoomLogic : IRoomLogic
{
    private IRoomRepository _repository;

    public RoomLogic(IRoomRepository repository)
    {
        _repository = repository;
    }

    public async Task AddRoom(string name, string deviceId, string homeId)
    {
        try
        {
            if (await _repository.CheckExistingRoom(deviceId, homeId))
            {
                await _repository.AddRoom(name, deviceId, homeId);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }

    public async Task DeleteRoom(string id)
    {
        try
        {
            if (await _repository.CheckNonExistingRoom(id))
            {
                await _repository.DeleteRoom(id);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }

    public async Task EditRoom(string id, string? name=null, string? deviceId=null)
    {
        try
        {
            if (await _repository.CheckNonExistingRoom(id))
            {
                await _repository.EditRoom(id,name,deviceId);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<Room>?> GetAllRooms(string homeId, string? deviceId=null)
    {
        try
        {
            return await _repository.GetAllRooms( homeId, deviceId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<RoomDataTransferDTO> GetRoomData(string homeId, string deviceId, bool temp=false, bool humi=false, bool light=false)
    {
        try
        {
            return await _repository.GetRoomData( homeId, deviceId, temp, humi, light);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}