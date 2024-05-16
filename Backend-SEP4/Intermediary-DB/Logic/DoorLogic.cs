using System.Security.Cryptography;
using System.Text;
using ConsoleApp1;
using DBComm.Logic.Interfaces;
using DBComm.Repository;

namespace DBComm.Logic;

public class DoorLogic : IDoorLogic
{
    private IDoorRepository _repository;
    private ICommunicator _communicator;
    public DoorLogic(IDoorRepository repository)
    {
        _repository = repository;
        _communicator = Communicator.Instance;
    }

    public async Task SwitchDoor(string houseId, string password, bool state)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        string hashedString = "";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }
       if (hashedString.Equals(await _repository.CheckPassword(houseId, password)) && _repository.CheckDoorState(houseId).Result != state)
        {
            await _communicator.SwitchDoor();
            await _repository.SaveDoorState(houseId, state);
        }
        else
        {
            throw new Exception("Password mismatch");
        }
    }

    public async Task ChangeLockPassword(string homeId, string password)
    {
        if (string.IsNullOrEmpty(homeId))
        {
            throw new Exception("House Id can not be empty");
        }
        
        try
        {
            if (await _repository.CheckIfDoorExist(homeId))
            {
               await _repository.ChangePassword(homeId, password); 
            }
            
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
}