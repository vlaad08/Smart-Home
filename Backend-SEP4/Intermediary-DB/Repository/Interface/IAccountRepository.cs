using DBComm.Shared;

namespace DBComm.Repository;

public interface IAccountRepository
{
    Task<Member> RegisterAdmin(string username, string password);
    Task<Member> RegisterMember(string username, string password);
    Task DeleteAccount(string username);
    Task EditUsername(string oldUsername, string newUsername);
    Task EditPassword(string username, string oldPassword, string newPassword);
    Task ToggleAdmin();

    Task<bool> CheckUser(string username);
}