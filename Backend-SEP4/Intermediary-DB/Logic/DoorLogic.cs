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
        string hashedString;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }

        string storedHashedPassword = await _repository.CheckHashedPassword(houseId);

        if (hashedString.Equals(storedHashedPassword))
        {
            bool currentState = await _repository.CheckDoorState(houseId);
            if (currentState != state)
            {
                await _communicator.SwitchDoor();
                await _repository.SaveDoorState(houseId, state);
            }
            else
            {
                if (currentState)
                {
                    throw new Exception("Door is already open.");
                }
                else
                {
                    throw new Exception("Door is already closed.");
                }
            }
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