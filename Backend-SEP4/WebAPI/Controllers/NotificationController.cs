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
    private INotificationLogic _logic;

    public NotificationController(INotificationLogic logic)
    {
        _logic = logic;
    }
    //An endpoint to get all the notifications of the house (request with homeId, returns all the  notifications (limit?? idk )) - limit 1 day
    [HttpGet, Route("{houseId}")]
    public async Task<ActionResult<List<Notification>>> GetNotifications([FromRoute] string houseId)
    {
        try
        {
            List<Notification>? notifications = await _logic.GetNotifications(houseId);
            return Ok(notifications);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}