using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using DBComm.Logic.Interfaces;
using DBComm.Repository;
using ECC;
using ECC.Interface;

namespace DBComm.Logic;

public class DoorLogic : IDoorLogic
{
    private IDoorRepository _repository;
    private TcpClient client;
    private NetworkStream stream;
    private IEncryptionService enc = new EncryptionService("S3cor3P45Sw0rD@f"u8.ToArray(),null);

    private INotificationRepository _notificationRepository;
    public DoorLogic(IDoorRepository repository )
    {
        DotNetEnv.Env.Load();
        string ServerAddress = Environment.GetEnvironmentVariable("SERVER_ADDRESS") ?? "192.168.137.209";

        this.client = new TcpClient(ServerAddress, 6868);
        stream = client.GetStream();
        byte[] messageBytes = enc.Encrypt("LOGIC CONNECTED:");
        stream.Write(messageBytes, 0, messageBytes.Length);
        this._repository = repository;
        _notificationRepository = new NotificationRepository(new Context());
    }

    public async Task SwitchDoor(string houseId, string password, bool state)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        string hashedString;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }
       if (hashedString.Equals(await _repository.CheckPassword(houseId, password)) && _repository.CheckDoorState(houseId).Result != state)
       {
            string deviceId = await _repository.GetFirstDeviceInHouse(houseId);
            // await _communicator.SwitchDoor();
            await _repository.SaveDoorState(houseId, state);
            int openClose = -1;
            if (state)
            {
                openClose = 1;
            }
            if (!state)
            {
                openClose = 0;
            }
            string message = $"LOGIC: {deviceId}30{openClose}            ";
            int blockSize = 16;
            int extraBytes = message.Length % blockSize;
            if (extraBytes != 0)
            {
                message = message.PadRight(message.Length + blockSize - extraBytes, ' ');
            }

            byte[] messageBytes = enc.Encrypt(message);
            await stream.WriteAsync(messageBytes, 0, messageBytes.Length);

        }
        else
        {
            await _notificationRepository.AddNotification(houseId, "Someone entered wrong password to your door!");
            throw new Exception("Password mismatch");
        }
    }


    public async Task ChangeLockPassword(string homeId, string password)
    {
        if (string.IsNullOrEmpty(homeId))
        {
            throw new Exception("House Id can not be empty");
        }
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        string hashedString;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }
        try
        {
            if (await _repository.CheckIfDoorExist(homeId))
            {
               await _repository.ChangePassword(homeId, hashedString); 
            }
            
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> GetDoorState(string houseId)
    {
        try
        {
            bool state = await _repository.CheckDoorState(houseId);
            return state;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}