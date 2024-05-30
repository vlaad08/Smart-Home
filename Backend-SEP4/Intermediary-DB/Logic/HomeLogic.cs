using System.ComponentModel.DataAnnotations;
using DBComm.Shared;

public class HomeLogic : IHomeLogic
{

     private IHomeRepository _repository;
    public HomeLogic(IHomeRepository repository)
    {
        _repository = repository;
    }
    public async Task<List<Member>> GetMembersByHomeId(string homeId)
    {
        return await _repository.GetMembersByHomeId(homeId);
    }

    public async Task AddMemberToHome(string username, string houseId)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ValidationException("Username null");
        }
        if (await _repository.CheckUserExists(username))
        {
            await _repository.AddMemberToHome(username, houseId);
        }
        else
        {
            throw new Exception("No user with that username");
        }
    }

    public async Task RemoveMemberFromHome(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ValidationException("Username cannot be null");
        }
        try
        {
            if (await _repository.CheckUserExists(username))
            {
               await _repository.RemoveMemberFromHome(username);
            }
            else
            {
                throw new Exception("User does not exist.");
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }

        return;
    }
}