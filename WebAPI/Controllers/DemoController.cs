using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController] [Route("demo")]
public class DemoController : ControllerBase
{

    [HttpGet("/")]
    public async Task<ActionResult> getString()
    {
        
        
        
        return Ok("got it");
    }
    
}