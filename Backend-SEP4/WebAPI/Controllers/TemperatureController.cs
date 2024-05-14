﻿using DBComm.Logic;
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
    [HttpGet, Route("History/{hardwareId}")]
    public async Task<ActionResult<ICollection<LightReading>>> getHistory([FromRoute] string hardwareId,
        [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        try
        {
            var lightHistory = await _temperatureLogic.getTemperatureHistory(hardwareId, dateFrom, dateTo);
            return Ok(lightHistory);
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost, Route("{hardwareId}/{level}")]
    public async Task<IActionResult> setTemperature([FromRoute] string hardwareId, [FromRoute ] int level)
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
    
    
}



















