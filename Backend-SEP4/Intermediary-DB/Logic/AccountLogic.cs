using System.ComponentModel.DataAnnotations;
using DBComm.Repository;
using DBComm.Shared;

namespace WebAPI.Service;

public class AccountLogic : IAccountLogic
{
    private IAccountRepository _repository;
    //maybe private
    public AccountLogic(IAccountRepository repository)
    {
        this._repository = repository;
    }
    public Task<Member> GetAdmin(string login, string password)
    {
        throw new NotImplementedException();
    }

    public async Task RegisterMember(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ValidationException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ValidationException("Password cannot be null");
        }
        await _repository.RegisterMember(username, password);

        return;
    }

    public async Task Delete(string username)
    {
        try
        {
            await _repository.DeleteAccount(username);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error deleting account: " + e.Message);
        }
    }

    public Task<Member> GetMember(string login, string password)
    {
        throw new NotImplementedException();
    }

    public async Task RegisterAdmin(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ValidationException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ValidationException("Password cannot be null");
        }
        await _repository.RegisterAdmin(username, password);
        
        return;
    }
}