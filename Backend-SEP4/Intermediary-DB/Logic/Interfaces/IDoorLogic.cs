namespace DBComm.Logic.Interfaces;

public interface IDoorLogic
{
    Task SwitchDoor(string houseId, string password, bool state);
    Task ChangeLockPassword(string homeId, string password);
    Task<bool> GetDoorState(string houseId);

}