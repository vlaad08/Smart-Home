using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("home")]
    [ApiController]
    [Authorize]
    public class HomeController : ControllerBase
    {
        
        private readonly IHomeLogic _logic;

        public HomeController(IHomeLogic logic)
        {
            _logic = logic;
        }
    
        // GET: home/{homeId}/members
        [HttpGet("{homeId}/members")]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers(string homeId)
        {
            var members = await _logic.GetMembersByHomeId(homeId);
            return Ok(members);
        }

        //An endpoint to add a house member account to the house (we send you houseId and username)
        [HttpPatch, Route("{houseId}/members/{username}"),  Authorize(Policy = "Admin")]
        public async Task<ActionResult> AddMemberToHome([FromRoute]string houseId, [FromRoute] string username)
        {
            try
            {
                await _logic.AddMemberToHome(username, houseId);
                return Ok("Member added to house.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

         //An endpoint to remove house member account from the house (we send you houseId and username of the user)
        [HttpPatch, Route("members/{username}"),  Authorize(Policy = "Admin")]
        public async Task<ActionResult> RemoveMemberFromHome([FromRoute] string username)
        {
            try
            {
                await _logic.RemoveMemberFromHome(username);
                return Ok("Member removed.");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}