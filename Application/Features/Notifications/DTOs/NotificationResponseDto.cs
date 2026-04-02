using System;

namespace Application.Features.Notifications.DTOs
{
    public record NotificationResponseDto(
        Guid Id,
        Guid UserId,
        string Message,
        bool IsRead,
        DateTime CreatedAt
    );
}
