using Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Application.Features.Notifications.Queries.GetUnreadNotificationsByUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly ISender _sender;

    public NotificationsController(ISender sender)
    {
        _sender = sender;
    }

    // GET: api/notifications/user/{userId}/unread
    [HttpGet("user/{userId:guid}/unread")]
    public async Task<IActionResult> GetUnread(Guid userId)
    {
        var result = await _sender.Send(new GetUnreadNotificationsByUserQuery(userId));
        return Ok(result);
    }

    // PATCH: api/notifications/{id}/read
    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await _sender.Send(new MarkNotificationAsReadCommand(id));
        return NoContent(); // HTTP 204: No Content (successful update without body response)
    }
}