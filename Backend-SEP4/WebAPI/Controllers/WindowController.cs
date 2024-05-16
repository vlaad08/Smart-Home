using DBComm.Logic;
using DBComm.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("window")]
[Authorize]
public class WindowController : ControllerBase
{
    private IWindowLogic _windowLogic;

    public WindowController(IWindowLogic windowLogic)
    {
        this._windowLogic = windowLogic;
    }

    [HttpPost("switch")]
    public async Task<ActionResult> SwitchWindow()
    {
        try
        {
            await _windowLogic.SwitchWindow();
            return Ok("Window switched successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}