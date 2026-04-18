using MediatR;
using System;

namespace Application.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public record MarkAllNotificationsAsReadCommand(Guid UserId) : IRequest<bool>;
}