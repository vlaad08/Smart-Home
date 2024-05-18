using DBComm.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("door")]
[Authorize]
public class DoorController : ControllerBase
{
    private IDoorLogic _logic;

    public DoorController(IDoorLogic logic)
    {
        _logic = logic;
    }


    //TODO: Implement the following endpoints
    //An endpoint to toggle the door for locking and unlocking (we send an object (houseId, boolean value))
    // [HttpPost, Route("houses/{houseId}/doors/switch")]
    // public async Task<IActionResult> SwitchDoor([FromRoute]string houseId, [FromBody] string password, [FromBody] bool state)
    // {
    //     try
    //     {
    //         await _logic.SwitchDoor(houseId, password, state);
    //         return Ok("Door unlocked.");
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return StatusCode(500, e.Message);
    //     }
    // }
    //An endpoint to change the password of the lock of the house (we send you house Id and the new password)



    //TODO: Check if the policies are set right form the authorization
    [HttpPut, Route("houses/{houseId}/doors/password")] //, Authorize(Policy = "Admin")
    public async Task<IActionResult> ChangePassword([FromRoute] string houseId, [FromBody] string newPassword)
    {
        try
        {
            await _logic.ChangeLockPassword(houseId, newPassword);
            return Ok("Password changed successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}