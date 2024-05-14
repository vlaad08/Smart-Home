using DBComm.Shared;

namespace DBComm.Repository;

public interface IAccountRepository
{
    Task<Member> RegisterAdmin(string username, string password);
    Task<Member> RegisterMember(string username, string password);
    Task DeleteAccount(string username);
    Task EditUsername(string oldUsername, string newUsername);
    Task EditPassword(string username, string oldPassword, string newPassword);
    Task ToggleAdmin(string username);

    Task<bool> CheckExistingUser(string username);
    Task<bool> CheckNonExistingUser(string username,string hash);
    Task<bool> CheckIfAdmin(string adminUsername,string hash,string username);
    Task RemoveMemberFromHouse(string username);
    Task<Member> Login(string username, string hash);
}