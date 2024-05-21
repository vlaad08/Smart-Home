using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Authorize]

[ApiController] [Route("humidity")]
public class HumidityController : ControllerBase
{
    private readonly IHumidityLogic _humidityLogic;

    public HumidityController(IHumidityLogic humidityLogic)
    {
        this._humidityLogic = humidityLogic;
    }

    [HttpGet("{hardwareId}/latest")]
    public async Task<ActionResult> GetLatestHumidity([FromRoute]string hardwareId)
    {
        try
        {
            HumidityReading? humidityReading = await _humidityLogic.getHumidity(hardwareId);
            return Ok(humidityReading);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpGet, Route("{hardwareId}/history")]
    public async Task<ActionResult<ICollection<LightReading>>> GetHumidityHistory([FromRoute] string hardwareId,
        [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        try
        {
            var humidityHistory = await _humidityLogic.getHumidityHistory(hardwareId, dateFrom, dateTo);
            return Ok(humidityHistory);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    //what happens wo async?
    [HttpPost, Route("devices/{deviceId}/{value}")]
    public async Task<ActionResult> SaveCurrentHumidityInRoom([FromRoute] string deviceId, [FromRoute] double value)
    {
        try
        {
            Console.WriteLine("humid 1");
            await _humidityLogic.saveHumidityReading(deviceId, value);
            Console.WriteLine("humid 2");
            return Ok("Humidity saved for all rooms in house");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
}