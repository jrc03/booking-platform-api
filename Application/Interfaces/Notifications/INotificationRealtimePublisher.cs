using Application.Features.Notifications.DTOs;

namespace Application.Interfaces.Notifications;

public interface INotificationRealtimePublisher
{
    Task PublishToUserAsync(Guid userId, NotificationResponseDto notification, CancellationToken cancellationToken = default);
}
