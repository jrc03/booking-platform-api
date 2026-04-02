using MediatR;
using System;

namespace Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public record MarkNotificationAsReadCommand(Guid NotificationId) : IRequest<bool>;
}
