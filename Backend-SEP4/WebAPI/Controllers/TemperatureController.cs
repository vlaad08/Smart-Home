using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("temperature")]
[Authorize]
public class TemperatureController : ControllerBase
{

    private readonly ITemperatureLogic _temperatureLogic;

    public TemperatureController(ITemperatureLogic temperatureLogic)
    {
        this._temperatureLogic = temperatureLogic;
    }

    [HttpGet("{deviceId}")]
    public async Task<ActionResult> GetLatestTemperature([FromRoute] string deviceId)
    {
        try
        {
            TemperatureReading? temperature = await _temperatureLogic.getLatestTemperature(deviceId);
            return Ok(temperature);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    //An endpoint to get the temperature history of a specific room based on id of that room (request with room id, returns a list of readings of temperature)
    [HttpGet, Route("{deviceId}/history")]
    public async Task<ActionResult<ICollection<LightReading>>> GetHistory([FromRoute] string deviceId,
        [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        try
        {
            var temperatureHistory = await _temperatureLogic.getTemperatureHistory(deviceId, dateFrom, dateTo);
            return Ok(temperatureHistory);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}



















