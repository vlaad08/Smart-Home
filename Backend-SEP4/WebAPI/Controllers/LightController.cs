using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("Light")]
public class LightController : ControllerBase
{

    private readonly ILightLogic _lightLogic;

    public LightController(ILightLogic lightLogic)
    {
        _lightLogic = lightLogic;
    }

    [HttpGet("{hardwareId}")]
    public async Task<ActionResult> getLatestLight([FromRoute]string hardwareId)
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

    [HttpGet, Route("History/{hardwareId}")]
    public async Task<ActionResult<ICollection<LightReading>>> getHistory([FromRoute] string hardwareId,
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
    
}