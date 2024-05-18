using System;
using System.Collections.Generic;
using DBComm.Logic;
using DBComm.Shared;
using Microsoft.EntityFrameworkCore;

namespace Intermediary_DB.Repository
{
    public class HomeRepository : IHomeRepository
    {

        public Context context;
        public HomeRepository(Context context)
        {
            this.context = context;
        }

        // Implement the methods from the IHomeRepository interface here
        public async Task<List<Member>> GetMembersByHomeId(string homeId)
        {
            var members = await context.member
                .Where(m => m.Home.Id == homeId)
                .ToListAsync();

    

            return members;
        }

        public async Task RemoveMemberFromHome(string username)
        {
            try
            {
                Member? existing = await context.member.Include(m => m.Home)
                    .SingleOrDefaultAsync(m => m.Username == username);
                if (existing != null)
                {
                    existing.Home = null;
                    context.member.Update(existing);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task AddMemberToHome(string username, string houseId)
        {
            try
            {
                Member? existing = await context.member.Include(m => m.Home)
                    .SingleOrDefaultAsync(m => m.Username == username);
                
                if (existing.Home != null)
                {
                    throw new Exception("Member is already assigned to a house");
                }

                Home home = await context.home.FindAsync(houseId);
                if (home == null)
                {
                    throw new Exception("No home w that id");
                }

                existing.Home = home;
                context.member.Update(existing);
                await context.SaveChangesAsync();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> CheckUserExists(string username)
        {
            Member? member = await context.member.Include(m=>m.Home).FirstOrDefaultAsync(m => m.Username == username);
            if (member != null)
            {
                return true;
            }
            return false;
        }
        
    }
}