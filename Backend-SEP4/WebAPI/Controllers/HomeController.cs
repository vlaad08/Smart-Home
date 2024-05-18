using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        
        private readonly IHomeLogic _homeLogic;

        public HomeController(IHomeLogic homeLogic)
        {
            this._homeLogic = homeLogic;
        }
    
        // GET: home/{homeId}/members
        [HttpGet("{homeId}/members")]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers(string homeId)
        {
            // TODO: Implement logic to retrieve members based on homeId
            // For now, let's return a dummy list of members
            var members = await _homeLogic.GetMembersByHomeId(homeId);

            return Ok(members);
        }

        //An endpoint to add a house member account to the house (we send you houseId and username)
        [HttpPatch, Route("{houseId}/members/{username}")]
        public async Task<ActionResult> AddMemberToHome([FromRoute]string houseId, [FromRoute] string username)
        {
            try
            {
                await _homeLogic.AddMemberToHome(username, houseId);
                return Ok("Member added to house.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

         //An endpoint to remove house member account from the house (we send you houseId and username of the user)
        [HttpPatch, Route("members/{username}")]
        public async Task<ActionResult> RemoveMemberFromHome([FromRoute] string username)
        {
            try
            {
                await _homeLogic.RemoveMemberFromHome(username);
                return Ok("Member removed.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}