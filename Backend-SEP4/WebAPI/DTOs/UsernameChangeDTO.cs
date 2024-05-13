namespace WebAPI.DTOs;

public class UsernameChangeDTO
{
    public string OldUsername { get; set; }
    public string NewUsername { get; set; }
    public string Password { get; set; }
}