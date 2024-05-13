using DBComm.Shared;

public interface IAccountLogic
{
    Task<Admin> GetAdmin(string login, string password);
    Task RegisterMember(string username, string password);
    Task<Member> GetMember(String login, string password);
    Task Delete(string username);

    Task<Member> GetAdmin(string login, string password);
    Task<Member> RegisterMember(string username, string password);
    Task<Member> GetMember(string login, string password);
    Task RegisterAdmin(string username, string password);

}