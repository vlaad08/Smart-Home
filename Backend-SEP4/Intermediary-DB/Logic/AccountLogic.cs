using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using DBComm.Repository;
using DBComm.Shared;

namespace WebAPI.Service;

public class AccountLogic : IAccountLogic
{
    private IAccountRepository _repository;
    public AccountLogic(IAccountRepository repository)
    {
        this._repository = repository;
    }

    private async Task<string> _hashPassword(string password)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        string hashedString = "";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }

        return hashedString;
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
            if (await _repository.CheckExistingUser(username))
            {
                string hash = await _hashPassword(password);
                Member member = await _repository.RegisterMember(username, hash);
                return member;
            }
        }catch(Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }

        return null;
    }

    public async Task Delete(string username,string password)
    {
        try
        {
            string hash = await _hashPassword(password);
            if (await _repository.CheckNonExistingUser(username,hash))
            {
                await _repository.DeleteAccount(username);
            }
        }
        catch (Exception e)
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
        try
        {
            if (await _repository.CheckExistingUser(username))
            {
                string hash = await _hashPassword(password);
                await _repository.RegisterAdmin(username, hash);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }

        return;
    }


    public async Task EditUsername(string oldUsername, string newUsername,string password)
    {
        try
        {
            string hash = await _hashPassword(password);
            if (await _repository.CheckNonExistingUser(oldUsername,hash))
            {
                await _repository.EditUsername(oldUsername, newUsername);
            }
            
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
            string hash = await _hashPassword(oldPassword);
            if (await _repository.CheckNonExistingUser(username,hash))
            {
                string newHash = await _hashPassword(newPassword);
                if (newHash == hash)
                {
                    throw new Exception("Cannot set same password");
                }
                await _repository.EditPassword(username, oldPassword, newHash);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }

    public async Task ToggleAdmin(string adminUsername,string adminPassword, string username)
    {
        try
        {
            string hash = await _hashPassword(adminPassword);
            if (await _repository.CheckIfAdmin(adminUsername,hash,username))
            {
                await _repository.ToggleAdmin(username);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
    }




    

    public async Task<Member> Login(string username, string password)
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
            string hash = await _hashPassword(password);
            return await _repository.Login(username, hash);
        }catch(Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(e.Message);
        }
        return null;
    }



   
}