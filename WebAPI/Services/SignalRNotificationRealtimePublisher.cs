using Application.Features.Notifications.DTOs;
using Application.Interfaces.Notifications;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Services;

public class SignalRNotificationRealtimePublisher : INotificationRealtimePublisher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationRealtimePublisher(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task PublishToUserAsync(Guid userId, NotificationResponseDto notification, CancellationToken cancellationToken = default)
    {
        return _hubContext.Clients.User(userId.ToString())
            .SendAsync("ReceiveNotification", notification, cancellationToken);
    }
}
