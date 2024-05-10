using DBComm.Shared;

namespace DBComm.Repository;

public interface IAccountRepository
{
    Task RegisterMember(Member member);
}