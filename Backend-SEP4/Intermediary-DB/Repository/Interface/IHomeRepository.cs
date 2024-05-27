using DBComm.Shared;

public interface IHomeRepository
{
    Task<List<Member>> GetMembersByHomeId(string homeId);
    Task AddMemberToHome(string username, string houseId);
    Task RemoveMemberFromHome(string username);
    Task<bool> CheckUserExists(string username);
}