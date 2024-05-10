using System.Security.Cryptography;
using System.Text;

namespace DBComm.Repository;

public class DoorRepository : IDoorRepository
{
    public DoorRepository()
    {
    }

    public async Task<string> CheckPassword(string password)
    {
        //Hardcoded password
        string pw = "12345678";
        byte[] inputBytes = Encoding.UTF8.GetBytes(pw);
        string hashedString = "";
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(inputBytes);
            hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
        }

        return hashedString;
    }
}