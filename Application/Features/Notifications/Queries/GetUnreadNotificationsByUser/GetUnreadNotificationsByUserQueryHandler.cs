using Application.Features.Notifications.DTOs;
using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Queries.GetUnreadNotificationsByUser
{
    public class GetUnreadNotificationsByUserQueryHandler : IRequestHandler<GetUnreadNotificationsByUserQuery, IEnumerable<NotificationResponseDto>>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUnreadNotificationsByUserQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationResponseDto>> Handle(GetUnreadNotificationsByUserQuery request, CancellationToken cancellationToken)
        {
            var notifications = await _notificationRepository.GetUnreadNotificationsByUserAsync(request.UserId);

            return notifications.Select(n => new NotificationResponseDto(
                n.Id,
                n.UserId,
                n.Message,
                n.IsRead,
                n.CreatedAt
            ));
        }
    }
}
