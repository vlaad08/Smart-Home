using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

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

    [HttpGet("{deviceId}")]
    public async Task<ActionResult> GetLatestTemperature([FromRoute] string deviceId)
    {
        try
        {

            Console.WriteLine("get hardware fasz");
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
        [FromBody] TimePeriodDTO dto)
    {
        try
        {
            var temperatureHistory = await _temperatureLogic.getTemperatureHistory(deviceId, dto.dateFrom, dto.dateTo);
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

    /*[HttpPost, Route("save")]
    public async Task<ActionResult> CallSaveTemperature()
    {
        
        try
        {
            HttpClient httpClient = new HttpClient();
            string deviceId = "1";
            double value = 20.0;

            string url = $"http://localhost:5084/temperature/devices/{deviceId}/{value}";

            HttpResponseMessage response = await httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Temperature saved successfully.");
                return Ok("Temperature saved successfully.");
            }
            else
            {
                Console.WriteLine("Failed to save temperature.");
                return StatusCode((int)response.StatusCode, "Failed to save temperature.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred while saving temperature: {ex.Message}");
            return StatusCode(500, $"Exception occurred while saving temperature: {ex.Message}");
        }
    }*/
}



















