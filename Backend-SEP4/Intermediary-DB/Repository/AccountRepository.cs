using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace DBComm.Repository;

public class AccountRepository : IAccountRepository
{
    public Context _context;
    public AccountRepository(Context context)
    {
        _context = context;
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
            Member? existing = await _context.member.FirstOrDefaultAsync(m=> m.Username == username);
            if (existing != null)
            {
                throw new Exception("Member with given username is already in the system.");
            }

            Member member = new Member(username, password);

            await _context.member.AddAsync(member);
            await _context.SaveChangesAsync();
            return member ;
        }
        catch(Exception e)
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