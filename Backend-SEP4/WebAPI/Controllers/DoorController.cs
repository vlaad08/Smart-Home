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

    [HttpPost, Route("{password}")]
    public async Task<IActionResult> SwitchDoor([FromRoute] string password)
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

}