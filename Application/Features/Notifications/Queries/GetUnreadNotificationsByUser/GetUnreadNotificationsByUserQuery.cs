using Application.Features.Notifications.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Features.Notifications.Queries.GetUnreadNotificationsByUser
{
    public record GetUnreadNotificationsByUserQuery(Guid UserId) : IRequest<IEnumerable<NotificationResponseDto>>;
}
