using System.Security.Cryptography;
using System.Text;
using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class DoorRepository : IDoorRepository
{
    public Context _context;
    public DoorRepository(Context context)
    {
        _context = context;
    }

    public async Task<string> CheckPassword(string houseId, string password)
    {
        Door? existing = await _context.door.Include(d => d.Home).SingleOrDefaultAsync(d=>d.Home.Id == houseId);
        if (existing != null)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            string hashedString = "";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                hashedString = BitConverter.ToString(hashBytes).Replace("-", "");
                return hashedString;
            }
        }

        return null;
    }
    
    public async Task<string> CheckHashedPassword(string houseId)
    {
        Door? existing = await _context.door.Include(d => d.Home).SingleOrDefaultAsync(d => d.Home.Id == houseId);
        if (existing != null)
        {
            return existing.LockPassword;
        }

        return null;
    }

    public async Task ChangePassword(string houseId, string password)
    {
        Door? existing = await _context.door.Include(d => d.Home).SingleOrDefaultAsync(d=>d.Home.Id == houseId);
        if (existing != null)
        {
            existing.LockPassword= password;
            _context.door.Update(existing);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CheckIfDoorExist(string homeId)
    {
       
        Door? door = await _context.door.Include(d=>d.Home).FirstOrDefaultAsync(d => d.Home.Id == homeId);
        if (door == null)
        {
            throw new Exception($"Door does not exist.");
        }
        return true;
    }

    public async Task<bool> CheckDoorState(string homeId)
    {
        Door? door = await _context.door.Include(d=>d.Home).FirstOrDefaultAsync(d => d.Home.Id == homeId);
        if (door == null)
        {
            throw new Exception($"Door does not exist.");
        }

        return door.IsOpen;
    }

    public async Task SaveDoorState(string houseId, bool state)
    {
        Door? door = await _context.door.Include(d=>d.Home).FirstOrDefaultAsync(d => d.Home.Id == houseId);
        if (door != null)
        {
            door.IsOpen = state;
            _context.door.Update(door);
            await _context.SaveChangesAsync();
        }
        else
        {
          throw new Exception($"Door does not exist.");  
        }
        
    }
}