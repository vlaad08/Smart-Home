namespace DBComm.Repository;

public interface IDoorRepository
{
    Task<string> CheckPassword(string houseId, string password);
    Task ChangePassword(string houseId, string password);
    Task<bool> CheckIfDoorExist(string homeId);
    Task SaveDoorState(string hardwareID, bool state);
    Task<bool> CheckDoorState(string homeId);
    Task<string> CheckHashedPassword(string houseId);
}