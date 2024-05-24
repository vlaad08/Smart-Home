using System.Net.Sockets;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using ECC;
using ECC.Interface;
using WebAPI.DTOs;

namespace DBComm.Logic;

public class RoomLogic : IRoomLogic
{
    private IRoomRepository _repository;
    private TcpClient client;
    private NetworkStream stream;
    private IEncryptionService enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(),null);
    // private ICommunicator _communicator;

    public RoomLogic(IRoomRepository repository)
    {
        this.client = new TcpClient("192.168.137.209", 6868);
        stream = client.GetStream();
        byte[] messageBytes = enc.Encrypt("LOGIC CONNECTED:");
        stream.Write(messageBytes, 0, messageBytes.Length);
        this._repository = repository;
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
//SEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEGGSEG
//add debug for shitty mitty wrong levels
    public async Task SetRadiatorLevel(string deviceId, int level)
    {
        if (level >= 1 && level <= 6)
        {
            await _repository.SetRadiatorLevel(deviceId, level);

            string message = $"LOGIC: {deviceId}10{level}            ";
            int blockSize = 16;
            int extraBytes = message.Length % blockSize;
            if (extraBytes != 0)
            {
                message = message.PadRight(message.Length + blockSize - extraBytes, ' ');
            }

            byte[] messageBytes = enc.Encrypt(message);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        }
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
            if ((_repository.GetWindowState(hardwareId).Result && !state) || (!_repository.GetWindowState(hardwareId).Result && state))
            {
                await _repository.SaveWindowState(hardwareId, state);
                int openClose = -1;
                if (state)
                {
                    openClose = 1;
                }
                if (!state)
                {
                    openClose = 0;
                }
                string message = $"LOGIC: {hardwareId}20{openClose}            ";
                int blockSize = 16;
                int extraBytes = message.Length % blockSize;
                if (extraBytes != 0)
                {
                    message = message.PadRight(message.Length + blockSize - extraBytes, ' ');
                }

                byte[] messageBytes = enc.Encrypt(message);
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
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
        if (level >= 0 && level <= 4)
        {
            await _repository.SetLightState(hardwareId, level);

            string message = $"LOGIC: {hardwareId}40{level}              ";
            int blockSize = 16;
            int extraBytes = message.Length % blockSize;
            if (extraBytes != 0)
            {
                message = message.PadRight(message.Length + blockSize - extraBytes, ' ');
            }

            byte[] messageBytes = enc.Encrypt(message);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
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