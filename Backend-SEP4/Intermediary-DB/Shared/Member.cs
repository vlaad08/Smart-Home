using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class Member
{
    [Key]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Home Home { get; set; }
    public Admin Admin { get; set; }

    public Member(string username, string password)
    {
        Username = username;
        Password = password;
    }
    public Member()
    {
        
    }
}