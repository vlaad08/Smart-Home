using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("Humidity")]
public class HumidityController : ControllerBase
{
    private readonly IHumidityLogic _humidityLogic;

    public HumidityController(IHumidityLogic humidityLogic)
    {
        this._humidityLogic = humidityLogic;
    }

    [HttpGet("{hardwareId}")]
    public async Task<ActionResult> getLatestTemperature([FromRoute]string hardwareId)
    {
        try
        {
            HumidityReading? temperature = await _humidityLogic.getHumidity(hardwareId);
            return Ok(temperature);
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
            var lightHistory = await _humidityLogic.getHumidityHistory(hardwareId, dateFrom, dateTo);
            return Ok(lightHistory);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}