namespace WebAPI.DTOs;

public class PasswordChangeDTO
{
    public string Username { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}