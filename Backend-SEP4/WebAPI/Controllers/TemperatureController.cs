using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("Temperature")]
public class TemperatureController : ControllerBase
{

    private readonly ITemperatureLogic _temperatureLogic;

    public TemperatureController(ITemperatureLogic temperatureLogic)
    {
        this._temperatureLogic = temperatureLogic;
    }

    [HttpGet("{hardwareId}")]
    public async Task<ActionResult> getLatestTemperature([FromRoute] string hardwareId)
    {
        try
        {
            TemperatureReading? temperature = await _temperatureLogic.getLatestTemperature(hardwareId);
            return Ok(temperature);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost("{hardwareId}/{temperatureLevel}")]
    public async Task<IActionResult> SetTemperatureLevel([FromRoute] string hardwareId, [FromRoute] int temperatureLevel)
    {
        try
        {
            _temperatureLogic.SetTemperatureLevel(hardwareId, temperatureLevel);
            return await Task.FromResult(Ok());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}