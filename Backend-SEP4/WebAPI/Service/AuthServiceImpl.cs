using DBComm.Shared;

namespace WebAPI.Service;

public class AuthServiceImpl : IAuthService
{
    public Task<Admin> GetAdmin(string login, string password)
    {
        throw new NotImplementedException();
    }

    public Task RegisterMember(Member member)
    {
        throw new NotImplementedException();
    }

    public Task<Member> GetMember(string login, string password)
    {
        throw new NotImplementedException();
    }
}