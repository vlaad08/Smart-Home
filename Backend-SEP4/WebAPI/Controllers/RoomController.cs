using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;
[ApiController]
[Route("rooms")]
[Authorize]
public class RoomController : ControllerBase
{
    private IRoomLogic logic;

    public RoomController(IRoomLogic logic)
    {
        this.logic = logic;
    }

    [HttpGet("{homeId}")]
    public async Task<ActionResult<List<Room>>> GetAllRooms([FromRoute] string homeId,[FromQuery]string? deviceId)
    {
        try
        {
            var rooms = await logic.GetAllRooms(homeId, deviceId);
            return Ok(rooms);
        }
        catch (Exception e)
        {
           return BadRequest(e.Message);
        }
    }

    [HttpGet("{homeId}/devices/{deviceId}")]
    public async Task<ActionResult<RoomDataTransferDTO>> GetRoomData([FromRoute] string homeId, [FromRoute] string deviceId, [FromQuery] bool temp,
        [FromQuery] bool humi, [FromQuery] bool light)
    {
        try
        {
            var data = await logic.GetRoomData(homeId, deviceId,temp,humi,light);
            return Ok(data);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost, Authorize(Policy = "Admin")]
    public async Task<ActionResult> AddRoom( [FromBody] RoomCreationDTO dto)
    {
        try
        {
            await logic.AddRoom(dto.name, dto.deviceId, dto.homeId);
            return Ok("Room created.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}"), Authorize(Policy = "Admin")]
    public async Task<ActionResult> DeleteRoom([FromRoute]string id)
    {
        try
        {
            await logic.DeleteRoom(id);
            return Ok("Room deleted.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{roomId}"), Authorize(Policy = "Admin")]
    public async Task<ActionResult> EditRoom([FromRoute] string roomId, [FromBody] RoomChangeDTO dto)
    {
        try
        {
            await logic.EditRoom(roomId,dto.Name, dto.DeviceId);
            return Ok("Room edited.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}