using DBComm.Shared;

public interface IAccountLogic
{

    Task Delete(string username, string password);
    Task EditUsername(string oldUsername, string newUsername,string password);
    Task EditPassword(string username, string oldPassword, string newPassword);
    Task ToggleAdmin(string adminUsername,string adminPassword, string username);
    Task<Member> RegisterMember(string username, string password);
    Task<Member> Login(string username, string password);
}
