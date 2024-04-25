using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("Temperature")]
public class TemperatureController : ControllerBase
{

    private readonly ITemperatureLogic _temperatureLogic;
    
    [HttpGet("/{hardwareId}")]
    public async Task<ActionResult> getLatestTemperature()
    {
        try
        {
            Temperature temperature = await _temperatureLogic.getTemperature();
            return Ok(temperature);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}