using DBComm.Logic.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("Door")]
public class DoorController : ControllerBase
{
    private IDoorLogic _logic;

    public DoorController(IDoorLogic logic)
    {
        _logic = logic;
    }

    [HttpPost, Route("doors/switch")]
    public async Task<IActionResult> SwitchDoor([FromBody] string password)
    {
        try
        {
            await _logic.SwitchDoor(password);
            return Ok("Done");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    [HttpPut, Route("doors/{houseId}/password")]
    public async Task<IActionResult> ChangePassword([FromRoute] string houseId, [FromBody] int newPassword)
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