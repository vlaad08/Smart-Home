using DBComm.Logic.Interfaces;
using DBComm.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("notifications")]
[Authorize]
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
            List<Notification>? notifications = await _notificationLogic.GetNotifications();
            return Ok(notifications);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}