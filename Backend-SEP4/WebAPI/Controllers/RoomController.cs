using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;
[ApiController]
[Route("Rooms")]
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
            return await logic.GetAllRooms(homeId, deviceId);
        }
        catch (Exception e)
        {
           return BadRequest(e.Message);
        }
    }

    [HttpGet("{homeId}/{deviceId}")]
    public async Task<ActionResult<RoomDataTransferDTO>> GetRoomData([FromRoute] string homeId, [FromRoute] string deviceId, [FromQuery] bool temp,
        [FromQuery] bool humi, [FromQuery] bool light)
    {
        try
        {
            return await logic.GetRoomData(homeId, deviceId,temp,humi,light);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddRoom( [FromBody] RoomCreationDTO dto)
    {
        try
        {
            await logic.AddRoom(dto.name, dto.deviceId, dto.homeId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRoom([FromRoute]string id)
    {
        try
        {
            await logic.DeleteRoom(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> EditRoom([FromRoute] string id, [FromQuery] string? deviceId,
        [FromQuery] string? name)
    {
        try
        {
            await logic.EditRoom(id,name,deviceId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}