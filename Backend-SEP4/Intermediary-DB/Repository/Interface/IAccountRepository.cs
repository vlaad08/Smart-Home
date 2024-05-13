using DBComm.Shared;

namespace DBComm.Repository;

public interface IAccountRepository
{
    Task<Member> RegisterAdmin(string username, string password);
    Task<Member> RegisterMember(string username, string password);
    Task DeleteAccount(string username);
}