using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet, Route("{hardwareId}/history")]
    public async Task<ActionResult<ICollection<LightReading>>> GetLightHistory([FromRoute] string hardwareId,
        [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        try
        {
            var lightHistory = await _lightLogic.getLightHistory(hardwareId, dateFrom, dateTo);
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
    
}