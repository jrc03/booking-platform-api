using Domain.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAllNotificationsAsReadCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var unreadNotifications = await _notificationRepository.GetUnreadNotificationsByUserAsync(request.UserId);

            if (unreadNotifications == null || !unreadNotifications.Any())
            {
                return true;
            }

            foreach (var notification in unreadNotifications)
            {
                notification.MarkAsRead();
                _notificationRepository.Update(notification);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}