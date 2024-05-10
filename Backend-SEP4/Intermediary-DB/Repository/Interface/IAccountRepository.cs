using DBComm.Shared;

namespace DBComm.Repository;

public interface IAccountRepository
{
    Task<Member> RegisterMember(string username, string password);
}