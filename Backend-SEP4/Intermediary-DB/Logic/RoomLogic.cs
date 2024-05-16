using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using WebAPI.DTOs;

namespace DBComm.Logic;

public class RoomLogic : IRoomLogic
{
    private IRoomRepository _repository;
    private ICommunicator _communicator;

    public RoomLogic(IRoomRepository repository)
    {
        _repository = repository;
        _communicator = Communicator.Instance;
    }

    public async Task AddRoom(string name, string deviceId, string homeId, int preferedTemperature, int preferedHumidity)
    {
        try
        {
            if (await _repository.CheckExistingRoom(deviceId, homeId))
            {
                await _repository.AddRoom(name, deviceId, homeId, preferedTemperature, preferedHumidity);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }

    public async Task DeleteRoom(string  deviceId)
    {
        try
        {
            if (await _repository.CheckNonExistingRoom(deviceId))
            {
                await _repository.DeleteRoom(deviceId);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }

    public async Task EditRoom(string id, string? name, string? deviceId, int? preferedTemperature, int? preferedHumidity)
    {
        try
        {
            if (await _repository.CheckNonExistingRoom(id))
            {
                await _repository.EditRoom(id,name,deviceId, preferedTemperature, preferedHumidity);
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<Room>?> GetAllRooms(string homeId)
    {
        try
        {
            return await _repository.GetAllRooms( homeId);
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

    public async Task SetRadiatorLevel(string deviceId, int level)
    {
        if (level >= 0 && level <= 6)
        {
           await _repository.SetRadiatorLevel(deviceId, level);
        }
        throw new Exception("Radiator level must be between 0 and 6.");
    }

    public Task<int> GetRadiatorLevel(string deviceId)
    {
        try
        {
            return _repository.GetRadiatorLevel(deviceId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }

    public async Task SaveWindowState(string hardwareId, bool state)
    {
        try
        {
            if (_repository.GetWindowState(hardwareId).Result && !state)
            {
                await _repository.SaveWindowState(hardwareId, state);
                await _communicator.SwitchWindow();
            }
            if(!_repository.GetWindowState(hardwareId).Result && state)
            {
              await _repository.SaveWindowState(hardwareId, state);
              await _communicator.SwitchWindow();
            }
            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> GetWindowState(string hardwareId)
    {
        try
        {
            return await _repository.GetWindowState(hardwareId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task SetLightState(string hardwareId, int level)
    {
        try
        {
            _communicator.setLight(hardwareId, level);
            if (level >= 0 && level <= 4)
            {
              await _repository.SetLightState(hardwareId, level);  
            } 
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task<int> GetLightState(string hardwareId)
    {
        try
        {
            return _repository.GetLightState(hardwareId);

        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}