using DBComm.Shared;

public interface IAccountLogic
{
    Task<Member> GetAdmin(string login, string password);
    Task<Member> RegisterMember(string username, string password);
    Task<Member> GetMember(String login, string password);

}