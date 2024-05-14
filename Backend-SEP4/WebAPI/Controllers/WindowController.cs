using DBComm.Logic;
using DBComm.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[ApiController]
[Route("Window")]
public class WindowController : ControllerBase
{
    private IWindowLogic _windowLogic;

    public WindowController(IWindowLogic windowLogic)
    {
        this._windowLogic = windowLogic;
    }

    [HttpPost]
    public async Task<ActionResult> SwitchWindow()
    {
        try
        {
            await _windowLogic.SwitchWindow();
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}