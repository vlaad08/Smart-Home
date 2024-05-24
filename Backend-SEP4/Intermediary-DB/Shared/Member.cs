using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DBComm.Shared;

public class Member
{
    [Key]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Home? Home { get; set; }
    public bool IsAdmin { get; set; }

    public Member(string username, string password, bool IsAdmin = false)
    {
        Id = Guid.NewGuid().ToString();
        Username = username;
        Password = password;
        this.IsAdmin = IsAdmin;
    }
    
    public Member()
    {
        
    }
}