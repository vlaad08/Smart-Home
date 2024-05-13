using DBComm.Shared;

public interface IAccountLogic
{
    Task<Member> GetAdmin(string login, string password);
    Task RegisterMember(string username, string password);
    Task<Member> GetMember(String login, string password);
    Task Delete(string username, string password);
    Task EditUsername(string oldUsername, string newUsername,string password);
    Task EditPassword(string username, string oldPassword, string newPassword);
    Task ToggleAdmin(String adminUsername,string adminPassword, string username);

}