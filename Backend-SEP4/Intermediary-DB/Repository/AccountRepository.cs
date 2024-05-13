using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql;

namespace DBComm.Repository;

public class AccountRepository : IAccountRepository
{
    public Context context;
    public AccountRepository(Context context)
    {
        this.context = context;
    }
    public async Task<Member> RegisterAdmin(string username, string password)
    {
         try
        {
            Member? existing = await _context.member.FirstOrDefaultAsync(m=> m.Username == username);
            if (existing != null)
            {
                throw new Exception("Member with given username is already in the system.");
            }

            Member member = new Member(username, password, true);


            await _context.member.AddAsync(member);
            await _context.SaveChangesAsync();
            return member ;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }   

    public async Task<Member> RegisterMember(string username, string password)
    {
        try
        {
            Member member = new Member(username,password);
            EntityEntry<Member> m = await context.member.AddAsync(member);
            await context.SaveChangesAsync();
            return m.Entity;
        }
        catch (Npgsql.NpgsqlException npgsqlEx)
        {
            throw new Exception($"Database operation failed: {npgsqlEx.Message}", npgsqlEx);
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred: {e.Message}", e);
        }
        return null;
    }

    public async Task EditUsername(string oldUsername, string newUsername)
    {
        Member member = await context.member.FirstOrDefaultAsync(m => m.Username == oldUsername);
        member.Username = newUsername;

        await context.SaveChangesAsync();
    }

    public async Task EditPassword(string username, string oldPassword, string newPassword)
    {
        Member member = await context.member.FirstOrDefaultAsync(m => m.Username == username);
        member.Password = newPassword;
        await context.SaveChangesAsync();
    }

    public async Task ToggleAdmin(string username)
    {
        Member member = await context.member.FirstOrDefaultAsync(m => m.Username == username);
        bool oldAdminValue = member.IsAdmin;
        member.IsAdmin = !oldAdminValue;
        await context.SaveChangesAsync();
    }

    public async Task<bool> CheckExistingUser(string username)
    {
        Member? member = await context.member.Include(m=>m.Home).FirstOrDefaultAsync(m => m.Username == username);
        if (member != null)
        {
            throw new Exception($"User with username {username} is already registered");
        }
        return true;
    }
    
    public async Task<bool> CheckNonExistingUser(string username,string hash)
    {
        Member? member = await context.member.Include(m=>m.Home).FirstOrDefaultAsync(m => m.Username == username);
        if (member == null)
        {
            throw new Exception($"User with username {username} doesn't exist");
        }
        else
        {
            if (member.Password!=hash)
            Member? existing = await _context.member.FirstOrDefaultAsync(m=> m.Username == username);
            if (existing != null)
            {
                throw new Exception("Password mismatch");
            }
        }
        return true;
    }

    public async Task<bool> CheckIfAdmin(string adminUsername, string hash,string username)
    {
        Member? member = await context.member.Include(m=>m.Home).FirstOrDefaultAsync(m => m.Username == adminUsername);
        if (member == null)
        {
            throw new Exception($"User with username {adminUsername} doesn't exist");
        }
        else
        {
            if (member.Password!=hash)
            {
                throw new Exception("Password mismatch");
            }
            else
            {
                if (!member.IsAdmin)
                {
                    throw new Exception($"User {adminUsername} is not an admin.");
                }
            }
        }
        Member? userToBeSet = await context.member.Include(m=>m.Home).FirstOrDefaultAsync(m => m.Username == username);
        if (userToBeSet == member)
        {
            throw new Exception("Admin cannot set themselves");
        }
        return true;
    }

    public async Task DeleteAccount(string username)
    {
        try
        {
            Member? acc = await context.member.FirstOrDefaultAsync(m=> m.Username == username);
            if ( acc == null)
            {
                throw new Exception("No account with this username");
            }

            context.member.Remove(acc);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task RemoveMemberFromHouse(string username, string houseId)
    {
        try
        {
            Member? existing = await _context.member.Include(m => m.Home)
                .SingleOrDefaultAsync(m => m.Username == username);
            if (existing != null)
            {
                /*Member member = new Member(existing.Username, existing.Password);
                member.Id = existing.Id;
                _context.member.Remove(existing);
                await _context.member.AddAsync(member);
                await _context.SaveChangesAsync();*/

                existing.Home = null;
                _context.member.Update(existing);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}