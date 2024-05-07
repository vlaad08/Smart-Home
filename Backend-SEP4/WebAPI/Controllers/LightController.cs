using DBComm.Logic;
using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("Light")]
public class LightController : ControllerBase
{

    private readonly ILightLogic _lightLogic;

    public LightController(ILightLogic lightLogic)
    {
        this._lightLogic = lightLogic;
    }

    [HttpGet("{hardwareId}")]
    public async Task<ActionResult> getLatestLight()
    {
        try
        {
            Light? light = await _lightLogic.getLight();
            return Ok(light);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}