using System.Net.Sockets;
using System.Text;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using ECC;
using ECC.Interface;

namespace DBComm.Logic;

public class TemperatureLogic : ITemperatureLogic
{
    private TcpClient client;
    private NetworkStream stream;
    private ITemperatureRepository _repository;
    private IEncryptionService enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(),null);

    public TemperatureLogic(ITemperatureRepository repository)
    {
        this.client = new TcpClient("192.168.137.1", 6868);
        stream = client.GetStream();
        byte[] messageBytes = enc.Encrypt("LOGIC CONNECTED:");
        stream.Write(messageBytes, 0, messageBytes.Length);
        this._repository = repository;
    }
    public async Task<TemperatureReading> GetLatestTemperature(string hardwareId)
    {
        return await _repository.GetLatestTemperature(hardwareId);
    }

    public void saveTemperature(TemperatureReading temperatureReading)
    {
        //repository.update(temperatureReading);
        
    }

    public async Task<ICollection<TemperatureReading>> GetTemperatureHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task setTemperature(string hardwareId, int level)
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
    
    public async Task SaveTempReading(string deviceId,double value)
    {
        DateTime dateTime = DateTime.UtcNow;
        await _repository.SaveTemperatureReading(deviceId,value, dateTime);
    }
}