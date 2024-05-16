using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("temperature")]
// [Authorize]
public class TemperatureController : ControllerBase
{

    private readonly ITemperatureLogic _temperatureLogic;

    public TemperatureController(ITemperatureLogic temperatureLogic)
    {
        this._temperatureLogic = temperatureLogic;
    }

    [HttpGet("{hardwareId}")]
    public async Task<ActionResult> GetLatestTemperature([FromRoute] string hardwareId)
    {
        try
        {
            Console.WriteLine("get hardware fasz");
            TemperatureReading? temperature = await _temperatureLogic.getLatestTemperature(hardwareId);
            return Ok(temperature);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    [HttpGet, Route("{hardwareId}/history")]
    public async Task<ActionResult<ICollection<LightReading>>> GetHistory([FromRoute] string hardwareId,
        [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        try
        {
            var temperatureHistory = await _temperatureLogic.getTemperatureHistory(hardwareId, dateFrom, dateTo);
            return Ok(temperatureHistory);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost, Route("{hardwareId}/set")]
    public async Task<IActionResult> SetTemperature([FromRoute] string hardwareId, [FromBody] int level)
    {
        if (level < 1 || level > 6)
        {
            return BadRequest("Level must be between 1 and 6.");
        }
        
        try
        {
            await _temperatureLogic.setTemperature(hardwareId, level);
            return Ok($"Light level set to {level} for hardware ID: {hardwareId}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost, Route("devices/{deviceId}/{value}")]
    public async Task<ActionResult> SaveCurrentTemperatureInRoom([FromRoute] string deviceId,[FromRoute] double value)
    {
        try
        {
            Console.WriteLine("Controller");
            await _temperatureLogic.saveTempReading(deviceId,value);
            Console.WriteLine("controller 2");
            return Ok($"Temperature saved for all rooms in house");
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    


}



















