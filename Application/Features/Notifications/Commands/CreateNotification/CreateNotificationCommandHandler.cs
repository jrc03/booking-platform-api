using Application.Features.Notifications.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Commands.CreateNotification
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, NotificationResponseDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<NotificationResponseDto> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = Notification.Create(
                userId: request.UserId,
                message: request.Message
            );

            _notificationRepository.Add(notification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new NotificationResponseDto(
                notification.Id,
                notification.UserId,
                notification.Message,
                notification.IsRead,
                notification.CreatedAt
            );
        }
    }
}
