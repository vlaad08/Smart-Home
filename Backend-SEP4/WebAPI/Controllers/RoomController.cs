using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
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
    //An endpoint to get all rooms based on a specific houseId (request with houseId, return a list of rooms with their ids, names, current temperature, humidity, light level)
    [HttpGet("{homeId}")]
    public async Task<ActionResult<List<Room>>> GetAllRooms([FromRoute] string homeId)
    {
        try
        {
            var rooms = await logic.GetAllRooms(homeId);
            return Ok(rooms);
        }
        catch (Exception e)
        {
           return BadRequest(e.Message);
        }
    }

    //TODO: Implement the following endpoints
    //n endpoint to get all room informations based on a specific deviceId (returns id, name, current temperature, humidity, light level)
    // [HttpGet("{deviceId}")]
    // public async Task<ActionResult<RoomDataTransferDTO>> GetRoomData([FromRoute] string homeId, [FromRoute] string deviceId, [FromQuery] bool temp,
    //     [FromQuery] bool humi, [FromQuery] bool light)
    // {
    //     try
    //     {
    //         var data = await logic.GetRoomData(homeId, deviceId,temp,humi,light);
    //         return Ok(data);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }
    //An endpoint to create a room (we send an object (roomName, hardwareId, default room temperature (number), default room humidity (number))
    [HttpPost, Authorize(Policy = "Admin")]
    public async Task<ActionResult> AddRoom( [FromBody] RoomCreationDTO dto)
    {
        try
        {
            await logic.AddRoom(dto.name, dto.deviceId, dto.homeId, dto.PreferedTemperature, dto.PreferedHumidity);
            return Ok("Room created.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    //An endpoint to remove a room (we send deviceId of the room to be deleted)
    [HttpDelete("{deviceId}"), Authorize(Policy = "Admin")]
    public async Task<ActionResult> DeleteRoom([FromRoute]string deviceId)
    {
        try
        {
            await logic.DeleteRoom(deviceId);
            return Ok("Room deleted.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    // room id because you can change the device id in that method, so the method should be done using roomId, which can not change during this method
    //An endpoint to edit a room (we send an object (roomId, new room name, new hardware id, new default temperature, new default humidity)

    [HttpPut("{roomId}"), Authorize(Policy = "Admin")]
    public async Task<ActionResult> EditRoom([FromRoute] string roomId, [FromBody] RoomChangeDTO dto)
    {
        try
        {
            await logic.EditRoom(roomId,dto.Name, dto.DeviceId, dto.PreferedTemperature, dto.PreferedHumidity);
            return Ok("Room edited.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    //An endpoint to set the radiator level (we send an object with deviceId and number 0-6)
    [HttpPost, Route("{deviceId}/radiator/set")]
    public async Task<IActionResult> SetRadiatorLevel([FromRoute]string deviceId, [FromBody] int level)
    {
        try
        {
            await logic.SetRadiatorLevel(deviceId, level);
            return Ok($"Radiator level set to {level} for hardware ID: {deviceId}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    //An endpoint to get the current radiator level based on deviceId(returns level of radiator (number 0-6))
    [HttpGet, Route("{deviceId}/radiator")]
    public async Task<ActionResult<int>> GetRadiatorLevel([FromRoute] string deviceId)
    {
        try
        {
            int level = await logic.GetRadiatorLevel(deviceId);
            return Ok(level);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    //An endpoint to set the light level 
    [HttpPost, Route("{deviceId}/light/set")]
    public async Task<ActionResult> SetLightLevel([FromRoute] string deviceId, [FromBody]int level)
    {
        try
        {
            await logic.SetRadiatorLevel(deviceId, level);
            return Ok("Light level set.");
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    //An endpoint to get current light level (request with device, returns number 0-4)
    [HttpGet, Route("{deviceId}/light")]
    public async Task<ActionResult<int>> GetLightLevel([FromRoute] string deviceId)
    {
        try
        {
            int level = await logic.GetLightState(deviceId);
            return Ok(level);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    //An endpoint to set the window state (we send an deviceId and true or false indicating if the windows are set to be open or close)
    [HttpPost, Route("{deviceId}/window/set")]
    public async Task<ActionResult> SetWindowState([FromRoute] string deviceId, [FromBody] bool state)
    {
        try
        {
            await logic.SaveWindowState(deviceId, state);
            return Ok("Window state is changed.");
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //An endpoint to get current window state based on deviceId (open/close)
    [HttpGet, Route("{deviceId}/window")]
    public async Task<ActionResult<bool>> GetWindowState([FromRoute] string deviceId)
    {
        try
        {
            bool state = await logic.GetWindowState(deviceId);
            return Ok(state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

}