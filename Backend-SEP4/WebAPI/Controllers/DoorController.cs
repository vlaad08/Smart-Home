using DBComm.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController]
[Route("door")]
[Authorize]
public class DoorController : ControllerBase
{
    private readonly IDoorLogic _logic;

    public DoorController(IDoorLogic logic)
    {
        _logic = logic;
    }

    //An endpoint to toggle the door for locking and unlocking (we send an object (houseId, boolean value))
    [HttpPost, Route("houses/{houseId}/doors/switch")]
    public async Task<IActionResult> SwitchDoor([FromRoute] string houseId, [FromBody] SwitchDoorDTO dto)
    {
        try
        {
            await _logic.SwitchDoor(houseId, dto.Password, dto.State);
            return Ok("Door state changed.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //an endpoint to get the door state based on haouse id
    [HttpGet, Route("houses/{houseId}")]
    public async Task<ActionResult<bool>> CheckDoorState([FromRoute] string houseId)
    {
        try
        {
            bool state = await _logic.GetDoorState(houseId);
            return Ok(state);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    //An endpoint to change the password of the lock of the house (we send you house Id and the new password)
    //TODO: Check if the policies are set right form the authorization
    [HttpPut, Route("houses/{houseId}/doors/password"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> ChangePassword([FromRoute] string houseId, [FromBody] string newPassword)
    {
        try
        {
            await _logic.ChangeLockPassword(houseId, newPassword);
            return Ok("Password changed successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

}