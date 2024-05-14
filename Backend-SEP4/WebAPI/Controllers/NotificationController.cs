using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("Notifications")]
public class NotificationController : ControllerBase
{
    private INotificationLogic _notificationLogic;

    public NotificationController(INotificationLogic notificationLogic)
    {
        this._notificationLogic = notificationLogic;
    }

    [HttpGet]
    public async Task<ActionResult<List<Notification>>> GetNotifications()
    {
        try
        {
            List<Notification>? temperature = await _notificationLogic.GetNotifications();
            return Ok(temperature);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}