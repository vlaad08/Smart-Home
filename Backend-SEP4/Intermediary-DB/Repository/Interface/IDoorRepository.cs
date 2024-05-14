namespace DBComm.Repository;

public interface IDoorRepository
{
    Task<string> CheckPassword(string password);
}