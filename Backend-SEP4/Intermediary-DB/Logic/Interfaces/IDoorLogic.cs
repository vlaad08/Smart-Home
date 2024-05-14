namespace DBComm.Logic.Interfaces;

public interface IDoorLogic
{
    Task SwitchDoor(string password);
    Task ChangeLockPassword(string homeId, int password);
}