using Application.Features.Bookings.Events;
using Application.Features.Notifications.DTOs;
using Application.Interfaces.Notifications;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Notifications.EventHandlers
{
    public class BookingCompletedNotificationHandler : INotificationHandler<BookingCompletedEvent>
    {

        private readonly IPropertyRepository _propertyRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationRealtimePublisher _notificationRealtimePublisher;
        private readonly IUnitOfWork _unitOfWork;

        public BookingCompletedNotificationHandler(
            IPropertyRepository propertyRepository,
            INotificationRepository notificationRepository,
            INotificationRealtimePublisher notificationRealtimePublisher,
            IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _notificationRepository = notificationRepository;
            _notificationRealtimePublisher = notificationRealtimePublisher;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(BookingCompletedEvent notification, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(notification.PropertyId);

            if (property == null) return;

            string guestMessage = $"Hope you enjoyed your stay at '{property.Title}'! Don't forget to leave a review.";
            var guestNotification = Notification.Create(notification.GuestId, guestMessage);

            string hostMessage = $"Your guest has successfully completed their stay at '{property.Title}'.";
            var hostNotification = Notification.Create(property.HostId, hostMessage);

            _notificationRepository.Add(guestNotification);
            _notificationRepository.Add(hostNotification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _notificationRealtimePublisher.PublishToUserAsync(
                guestNotification.UserId,
                new NotificationResponseDto(
                    guestNotification.Id,
                    guestNotification.UserId,
                    guestNotification.Message,
                    guestNotification.IsRead,
                    guestNotification.CreatedAt
                ),
                cancellationToken);

            await _notificationRealtimePublisher.PublishToUserAsync(
                hostNotification.UserId,
                new NotificationResponseDto(
                    hostNotification.Id,
                    hostNotification.UserId,
                    hostNotification.Message,
                    hostNotification.IsRead,
                    hostNotification.CreatedAt
                ),
                cancellationToken);


        }
    }
}