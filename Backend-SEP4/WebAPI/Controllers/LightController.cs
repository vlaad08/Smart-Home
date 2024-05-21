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

    private readonly ILightLogic _lightLogic;

    public LightController(ILightLogic lightLogic)
    {
        _lightLogic = lightLogic;
    }

    [HttpGet("{hardwareId}/latest")]
    public async Task<ActionResult> GetLatestLight([FromRoute]string hardwareId)
    {
        try
        {
            LightReading? light = await _lightLogic.getLight(hardwareId);
            return Ok(light);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    //An endpoint to get the light level history of a specific room based on id of that room (request with room id, returns a list of readings of light levels)
    [HttpGet, Route("{hardwareId}/history")]
    public async Task<ActionResult<ICollection<LightReading>>> GetLightHistory([FromRoute] string hardwareId,
        [FromBody] TimePeriodDTO dto)
    {
        try
        {
            var lightHistory = await _lightLogic.getLightHistory(hardwareId, dto.dateFrom, dto.dateTo);
            return Ok(lightHistory);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost, Route("{hardwareId}/level")]
    public async Task<ActionResult> SetLight([FromRoute] string hardwareId, [FromBody]int level)
    {
        try
        {
            await _lightLogic.SetLight(hardwareId, level);
            return Ok("Light level set.");
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost, Route("devices/{deviceId}/{value}")]
    public async Task<ActionResult> SaveCurrentLightInRoom([FromRoute] string deviceId, [FromRoute] double value)
    {
        try
        {
            Console.WriteLine("Controller");
            await _lightLogic.saveLightReading(deviceId, value);
            Console.WriteLine("controller 2");
            return Ok($"Temperature saved for all rooms in house");
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
}