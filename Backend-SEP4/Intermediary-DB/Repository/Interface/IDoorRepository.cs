namespace DBComm.Repository;

public interface IDoorRepository
{
    Task<string> CheckPassword(string password);
    Task ChangePassword(string houseId, string password);
    Task<bool> CheckIfDoorExist(string homeId);
}