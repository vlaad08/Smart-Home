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
        try
        {
            if (await _repository.CheckUser(username))
            {
                await _repository.RegisterMember(username, password);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
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
        try
        {
            if (await _repository.CheckUser(username))
            {
                await _repository.RegisterAdmin(username, password);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }

        return;
    }

    public async Task EditUsername(string oldUsername, string newUsername)
    {
        try
        {
            await _repository.EditUsername(oldUsername, newUsername);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task EditPassword(string username,string oldPassword, string newPassword)
    {
        try
        {
            await _repository.EditPassword(username, oldPassword, newPassword);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task ToggleAdmin()
    {
        try
        {
            await _repository.ToggleAdmin();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }
}