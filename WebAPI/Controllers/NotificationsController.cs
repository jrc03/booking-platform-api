using Application.Features.Notifications.Commands.MarkNotificationAsRead;
using Application.Features.Notifications.Queries.GetUnreadNotificationsByUser;
using Application.Interfaces.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ICurrentUserService _currentUserService;

    public NotificationsController(ISender sender, ICurrentUserService currentUserService)
    {
        _sender = sender;
        _currentUserService = currentUserService;
    }

    // GET: api/notifications/unread
    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Usuario no válido.");
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