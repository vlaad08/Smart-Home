using DBComm.Logic.Interfaces;
using DBComm.Repository;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;

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
    //An endpoint to get the humidity history of a specific room based on id of that room (request with room id, returns a list of readings of humidity)
    [HttpGet, Route("{hardwareId}/history")]
    public async Task<ActionResult<ICollection<LightReading>>> GetHumidityHistory([FromRoute] string hardwareId,
        [FromBody] TimePeriodDTO dto)
    {
        try
        {
            var humidityHistory = await _humidityLogic.getHumidityHistory(hardwareId, dto.dateFrom, dto.dateTo);
            return Ok(humidityHistory);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}