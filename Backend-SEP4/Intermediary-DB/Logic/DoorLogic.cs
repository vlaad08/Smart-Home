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
        this._repository = repository;
        _communicator = Communicator.Instance;
    }

    public async Task SwitchDoor(string password)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        string hashedString = "";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }
        if (hashedString.Equals(await _repository.CheckPassword(password)))
        {
            await _communicator.SwitchDoor();
        }
        else
        {
            throw new Exception("Password mismatch");
        }
    }
}