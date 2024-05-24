using System.Net.Sockets;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using ECC;
using ECC.Interface;

namespace DBComm.Logic;

public class LightLogic : ILightLogic
{
    private TcpClient client;
    private NetworkStream stream;
    private ILigthRepository _repository;
    private IEncryptionService enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(),null);
    // private ICommunicator _communicator;

    public LightLogic(ILigthRepository repository)
    {
        this.client = new TcpClient("192.168.137.209", 6868);
        stream = client.GetStream();
        byte[] messageBytes = enc.Encrypt("LOGIC CONNECTED:");
        stream.Write(messageBytes, 0, messageBytes.Length);
        
        this._repository = repository;
        // _communicator = Communicator.Instance;
    }
    public async Task<LightReading> GetLatestLight(string hardwareId)
    {
        return await _repository.GetLatestLight(hardwareId);
    }

    public async Task<ICollection<LightReading>> GetLightHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task SetLight(string hardwareId, int level)
    {
        string message = $"LOGIC: {hardwareId}{level}              ";
        int blockSize = 16; 
        int extraBytes = message.Length % blockSize;
        if (extraBytes != 0)
        {
            message = message.PadRight(message.Length + blockSize - extraBytes, ' ');
        }
        byte[] messageBytes = enc.Encrypt(message);
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
    }
    
    public async Task SaveLightReading(string deviceId,double value)
    {
        DateTime readAt = DateTime.UtcNow;
        await _repository.SaveLightReading(deviceId, value, readAt);
    }
}