﻿using System.Net.Sockets;
using System.Text;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using ECC;
using ECC.Interface;
using WebAPI.DTOs;

namespace DBComm.Logic;

public class TemperatureLogic : ITemperatureLogic
{
    private TcpClient client;
    private NetworkStream stream;
    private ITemperatureRepository _repository;
    private IEncryptionService enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(),null);
    private IRoomRepository _roomRepository;
    private INotificationRepository _notificationRepository;
    public bool writeAsyncCalled { get; set; }

    public TemperatureLogic(ITemperatureRepository repository, TcpClient? c = null)
    {

        DotNetEnv.Env.Load();
        string ServerAddress = Environment.GetEnvironmentVariable("SERVER_ADDRESS") ?? "0.0.0.0";
        this.client = c ?? new TcpClient(ServerAddress, 6868);

        stream = client.GetStream();
        byte[] messageBytes = enc.Encrypt("LOGIC CONNECTED:");
        stream.Write(messageBytes, 0, messageBytes.Length);
        this._repository = repository;
        this._roomRepository = new RoomRepository(new Context());
        this._notificationRepository = new NotificationRepository(new Context());
    }
    public async Task<TemperatureReading> GetLatestTemperature(string hardwareId)
    {
        return await _repository.GetLatestTemperature(hardwareId);
    }

    public async Task<ICollection<TemperatureReading>> GetTemperatureHistory(string hardwareId, DateTime dateFrom, DateTime dateTo)
    {
        return await _repository.GetHistory(hardwareId, dateFrom, dateTo);
    }

    public async Task SetTemperature(string hardwareId, int level)
    {
        if (level < 0 || level > 6)
        {
            throw new Exception("Level must be between 1 and 6.");
        }
        
        string message = $"LOGIC: {hardwareId}{level}              ";
        int blockSize = 16; 
        int extraBytes = message.Length % blockSize;
        if (extraBytes != 0)
        {
            message = message.PadRight(message.Length + blockSize - extraBytes, ' ');
        }
        byte[] messageBytes = enc.Encrypt(message);
        writeAsyncCalled = false;
        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
        writeAsyncCalled = true;
    }

    public async Task SaveTempReading(string deviceId, double value)
    {
        RoomDataDTO dto = await _roomRepository.GetRoomData(null, deviceId);
        if (dto == null)
        {
            throw new InvalidOperationException("Room data is null.");
        }

        if (dto.Home == null)
        {
            throw new InvalidOperationException("Home data is null.");
        }

        if ((double)dto.PreferedTemperature < value)
        {
            List<Notification> notifications = await _notificationRepository.GetNotifications(dto.Home.Id);

            if (notifications == null || notifications.Count == 0)
            {
                await _notificationRepository.AddNotification(dto.Home.Id,
                    "The temperature in " + dto.Name + " is higher than preferred and it is " + value + " degrees");
            }
            else
            {
                bool exists = notifications.Any(n =>
                    n.Message.StartsWith("The temperature in " + dto.Name + " is higher than preferred"));
                Console.WriteLine(exists);
                if (!exists)
                {
                    await _notificationRepository.AddNotification(dto.Home.Id,
                        "The temperature in " + dto.Name + " is higher than preferred and it is " + value + " degrees");
                }
            }
        }
        
        DateTime dateTime = DateTime.UtcNow.AddHours(2);
        await _repository.SaveTemperatureReading(deviceId, value, dateTime);
    }
}