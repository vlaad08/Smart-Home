namespace DBComm.Repository;

public interface IDoorRepository
{
    Task<string> CheckPassword(string password);
    Task ChangePassword(string houseId, int password);
    Task<bool> CheckIfDoorExist(string homeId);
}