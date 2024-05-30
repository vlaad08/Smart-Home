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

    public bool writeAsyncCalled { get; set; }
    public DoorLogic(IDoorRepository repository, TcpClient? c = null)
    {
        DotNetEnv.Env.Load();
        string ServerAddress = Environment.GetEnvironmentVariable("SERVER_ADDRESS") ?? "127.0.0.1";

        this.client = c ?? new TcpClient(ServerAddress, 6868);

        stream = client.GetStream();
        byte[] messageBytes = enc.Encrypt("LOGIC CONNECTED:");
        stream.Write(messageBytes, 0, messageBytes.Length);
        this._repository = repository;
        _notificationRepository = new NotificationRepository(new Context());
    }

    public DoorLogic(IDoorRepository repository, INotificationRepository notificationRepository, TcpClient? c = null)
    {
        _repository = repository;
        _notificationRepository = notificationRepository;
        this.client = c ?? new TcpClient("192.168.137.209", 6868);
        stream = client.GetStream();
        byte[] messageBytes = enc.Encrypt("LOGIC CONNECTED:");
        stream.Write(messageBytes, 0, messageBytes.Length);
        this._repository = repository;

    public async Task SwitchDoor(string houseId, string password, bool state)
    {
        try
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
                writeAsyncCalled = false;
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                writeAsyncCalled = true;

            }
            else
            {
                await _notificationRepository.AddNotification(houseId, "Someone entered wrong password to your door!");
                throw new Exception("Password mismatch");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }


    public async Task ChangeLockPassword(string homeId, string password)
    {
        if (string.IsNullOrEmpty(homeId))
        {
            throw new Exception("House Id can not be empty");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new Exception("Password cannot be null");
        }
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        string hashedString;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }

        if (password.Length == 0)
        {
            throw new Exception("Password can not be empty");
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