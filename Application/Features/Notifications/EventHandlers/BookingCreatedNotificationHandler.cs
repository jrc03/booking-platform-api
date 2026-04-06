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
    public class BookingCreatedNotificationHandler : INotificationHandler<BookingCreatedEvent>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingCreatedNotificationHandler(IPropertyRepository propertyRepository, INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
        {
            // 1. Fetch the PROPERTY to know who the owner (Host) is
            var property = await _propertyRepository.GetByIdAsync(notification.PropertyId);
            
            if (property == null) return; // Safety check

            // 2. Create the message text for the host
            string message = $"You have a new confirmed booking for '{property.Title}'! Expected total earnings: ${notification.TotalPrice}";

            // 3. Use the factory to create the notification, assigned to the property owner (HostId)
            var newNotification = Notification.Create(property.HostId, message);

            // 4. Save the new notification to the database
            _notificationRepository.Add(newNotification);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}