using DBComm.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("Temperature")]
public class TemperatureController : ControllerBase
{

    private readonly ITemperatureLogic _temperatureLogic;
    
    [HttpGet("/")]
    public async Task<ActionResult> get()
    {
        try
        {
            Console.WriteLine("segg");
            _temperatureLogic.getTemp();
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}