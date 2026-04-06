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
    public class BookingCancelledNotificationHandler : INotificationHandler<BookingCancelledEvent>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingCancelledNotificationHandler(IPropertyRepository propertyRepository, INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(BookingCancelledEvent notification, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(notification.PropertyId);

            if (property == null) return; // Safety check

            // 2. Create the message text for the host and the guest
            string hostMessage = $"The booking for your property '{property.Title}' has been cancelled.";
            var hostNotification = Notification.Create(property.HostId, hostMessage);

            string guestMessage = $"Your booking for '{property.Title}' has been successfully cancelled.";
            var guestNotification = Notification.Create(notification.GuestId, guestMessage);

            // 3. Save both notifications to the database
            _notificationRepository.Add(hostNotification);
            _notificationRepository.Add(guestNotification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}