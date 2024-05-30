using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

namespace WebAPI.Controllers;

[ApiController] [Route("light")]
[Authorize]
public class LightController : ControllerBase
{

    private readonly ILightLogic _logic;

    public LightController(ILightLogic logic)
    {
        _logic = logic;
    }

    [HttpGet("{hardwareId}/latest")]
    public async Task<ActionResult> GetLatestLight([FromRoute]string hardwareId)
    {
        try
        {
            LightReading? light = await _logic.GetLatestLight(hardwareId);
            return Ok(light);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    //An endpoint to get the light level history of a specific room based on id of that room (request with room id, returns a list of readings of light levels)
    [HttpGet, Route("{hardwareId}/history")]
    public async Task<ActionResult<ICollection<LightReading>>> GetLightHistory([FromRoute] string hardwareId,
        [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo )
    {
        try
        {
            var lightHistory = await _logic.GetLightHistory(hardwareId, dateFrom, dateTo);
            return Ok(lightHistory);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost, Route("{hardwareId}/level")]
    public async Task<ActionResult> SetLight([FromRoute] string hardwareId, [FromBody]int level)
    {
        if (level < 0 || level > 4)
        {
            return BadRequest("Light level must be between 0-4");
        }
        try
        {
            await _logic.SetLight(hardwareId, level);
            return Ok("Light level set.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost, Route("devices/{deviceId}/{value}"),AllowAnonymous]

    public async Task<ActionResult> SaveCurrentLightInRoom([FromRoute] string deviceId, [FromRoute] double value)
    {
        try
        {
            await _logic.SaveLightReading(deviceId, value);
            return Ok($"Light level saved");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}