using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.NotificationId) ?? throw new Exception($"Notification with ID {request.NotificationId} not found.");
            
            notification.MarkAsRead();

            _notificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
