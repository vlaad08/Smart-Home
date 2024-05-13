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
            Member member = new Member(username, password, true);
            await context.member.AddAsync(member);
            await context.SaveChangesAsync();
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
        if (member == null)
        {
            throw new Exception($"User with username {oldUsername} is not registered");
        }

        member.Username = newUsername;

        await context.SaveChangesAsync();
    }

    public Task EditPassword(string username, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task ToggleAdmin()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CheckUser(string username)
    {
        Member member = await context.member.FirstOrDefaultAsync(m => m.Username == username);
        if (member != null)
        {
            throw new Exception($"User with username {username} is already registered");
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
}