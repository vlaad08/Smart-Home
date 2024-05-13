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

    public async Task<Member> RegisterMember(string username, string password)
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
            return await _repository.RegisterMember(username, password);

        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
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

    public async Task RemoveMemberFromHouse(string username, string homeId)
    {
        try
        {
            //add check if user exists 
            await _repository.RemoveMemberFromHouse(username, homeId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
}