using System.ComponentModel.DataAnnotations;
using DBComm.Shared;

namespace WebAPI.Service;

public class AccountLogic : IAccountService
{
    public Task<Admin> GetAdmin(string login, string password)
    {
        throw new NotImplementedException();
    }

    public Task RegisterMember(Member member)
    {
        if (string.IsNullOrEmpty(member.Username))
        {
            throw new ValidationException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(member.Password))
        {
            throw new ValidationException("Password cannot be null");
        }
        // Do more user info validation here
        
        // save to persistence instead of list
        
       // member.Add(member);
        
        return Task.CompletedTask;
    }

    public Task<Member> GetMember(string login, string password)
    {
        throw new NotImplementedException();
    }
}