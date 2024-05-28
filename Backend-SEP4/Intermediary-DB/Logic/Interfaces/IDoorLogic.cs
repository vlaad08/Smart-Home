namespace DBComm.Logic.Interfaces;

public interface IDoorLogic
{
    Task SwitchDoor(string password);
}