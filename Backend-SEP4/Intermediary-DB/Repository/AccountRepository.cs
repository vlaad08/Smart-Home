using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DBComm.Repository;

public class AccountRepository : IAccountRepository
{
    public Context Context;
    public AccountRepository(Context context)
    {
        Context = context;
    }
    public async Task<Member> RegisterMember(string username, string password)
    {
        try
        {
            Member? existing = await Context.member.FirstOrDefaultAsync(m=> m.Username == username);
            if (existing != null)
            {
                throw new Exception("Member with given username is already in the system.");
            }

            Member member = new Member(username, password);

            await Context.member.AddAsync(member);
            await Context.SaveChangesAsync();
            return member ;
        }
        catch(Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task DeleteAccount(string username)
    {
        try
        {
            Member? acc = await Context.member.FirstOrDefaultAsync(m=> m.Username == username);
            if ( acc == null)
            {
                throw new Exception("No account with this username");
            }

            Context.member.Remove(acc);
            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}