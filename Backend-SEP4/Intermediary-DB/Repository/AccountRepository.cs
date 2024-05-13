using System.Data;
using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace DBComm.Repository;

public class AccountRepository : IAccountRepository
{
    public Context Context;
    public AccountRepository(Context context)
    {
        Context = context;
    }

   

    public async Task<Member> RegisterAdmin(string username, string password)
    {
         try
        {
            Member? existing = await Context.member.FirstOrDefaultAsync(m=> m.Username == username);
            if (existing != null)
            {
                throw new Exception("Member with given username is already in the system.");
            }

            Member member = new Member(username, password, true);


            await Context.member.AddAsync(member);
            await Context.SaveChangesAsync();
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
}