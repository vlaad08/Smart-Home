using System.ComponentModel.DataAnnotations;

namespace DBComm.Shared;

public class Admin
{
    [Key]
    public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public Home Home { get; set; }

    public Admin()
    {
        
    }
}