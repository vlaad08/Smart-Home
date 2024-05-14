using DBComm.Shared;

public interface IAccountLogic
{

    Task Delete(string username, string password);
    Task EditUsername(string oldUsername, string newUsername,string password);
    Task EditPassword(string username, string oldPassword, string newPassword);
    Task ToggleAdmin(String adminUsername,string adminPassword, string username);

    Task<Member> GetAdmin(string login, string password);
    Task<Member> RegisterMember(string username, string password);
    Task<Member> GetMember(string login, string password);
    Task RegisterAdmin(string username, string password);
    Task RemoveMemberFromHouse(string username);
<<<<<<< HEAD
    Task<Member> Login(string username, string password);
=======
    Task AddMemberToHouse(string username, string houseId);
>>>>>>> BACKEND
}
