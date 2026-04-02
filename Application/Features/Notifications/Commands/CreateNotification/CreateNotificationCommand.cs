using Application.Features.Notifications.DTOs;
using MediatR;
using System;

namespace Application.Features.Notifications.Commands.CreateNotification
{
    public record CreateNotificationCommand(
        Guid UserId,
        string Message
    ) : IRequest<NotificationResponseDto>;
}
