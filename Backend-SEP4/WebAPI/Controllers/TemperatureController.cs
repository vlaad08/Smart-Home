﻿using DBComm.Logic;
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

    private readonly ITemperatureLogic _logic;

    public TemperatureController(ITemperatureLogic logic)
    {
        _logic = logic;
        
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
    public async Task<ActionResult<ICollection<LightReading>>> GetHistory([FromRoute] string deviceId, [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
    {
        try
        {
            var temperatureHistory = await _logic.GetTemperatureHistory(deviceId, dateFrom, dateTo);
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

    [HttpPost, Route("devices/{deviceId}/{value}"),AllowAnonymous]
    public async Task<ActionResult> SaveCurrentTemperatureInRoom([FromRoute] string deviceId,[FromRoute] double value)
    {
        try
        {
            await _logic.SaveTempReading(deviceId,value);
            return Ok($"Temperature saved.");
        }catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}



















