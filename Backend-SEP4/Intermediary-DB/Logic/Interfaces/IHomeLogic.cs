using DBComm.Shared;

public interface IHomeLogic
{
    Task<List<Member>> GetMembersByHomeId(string homeId);
    Task AddMemberToHome(string username, string houseId);
    Task RemoveMemberFromHome(string username);

}