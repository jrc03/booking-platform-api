using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Bookings.Events;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Notifications.EventHandlers
{
    public class BookingCompletedNotificationHandler : INotificationHandler<BookingCompletedEvent>
    {

        private readonly IPropertyRepository _propertyRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingCompletedNotificationHandler(IPropertyRepository propertyRepository, INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(BookingCompletedEvent notification, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(notification.PropertyId);

            if (property == null) return;

            string guestMessage = $"Hope you enjoyed your stay at '{property.Title}'! Don't forget to leave a review.";
            var guestNotification = Notification.Create(notification.GuestId, guestMessage);

            // Save the notification to the database (Only Guest should be notified on completion)
            _notificationRepository.Add(guestNotification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);


        }
    }
}