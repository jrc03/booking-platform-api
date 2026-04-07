using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Bookings.DTOs;
using Application.Features.Bookings.Events;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;
using Application.Interfaces.Auth;

namespace Application.Features.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponseDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisher _publisher;
        private readonly ICurrentUserService _currentUserService;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, IPropertyRepository propertyRepository, IUnitOfWork unitOfWork, IPublisher publisher, ICurrentUserService currentUserService)
        {
            _bookingRepository = bookingRepository;
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
            _currentUserService = currentUserService;
        }

        public async Task<BookingResponseDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var guestId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Invalid user.");

            var property = await _propertyRepository.GetByIdAsync(request.PropertyId) ?? throw new Exception("Property not found");

            if (property.HostId == guestId)
                throw new InvalidOperationException("Cannot book your own property");

            var requestedDates = new DateRange(request.StartDate, request.EndDate);

            // Availability Rule 1 & 2: Manually Blocked Dates + Confirmed Bookings
            if (property.BlockedDates.Any(b => b.Overlaps(requestedDates)))
                throw new InvalidOperationException("The property is no longer available for the selected dates. (Blocked by Host)");

            var isOccupied = await _bookingRepository.HasOverlappingBookingsAsync(property.Id, requestedDates);
            if (isOccupied)
                throw new InvalidOperationException("The property is no longer available for the selected dates.");

            var newBooking = Booking.Create(
                request.PropertyId,
                guestId,
                requestedDates,
                property.PricePerNight
            );

            // Concurrency Token Trigger
            property.RecordBooking();
            _propertyRepository.Update(property);

            _bookingRepository.Add(newBooking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new BookingCreatedEvent(
            newBooking.Id,
            newBooking.PropertyId,
            newBooking.GuestId,
            newBooking.TotalPrice
            ), cancellationToken);

            return new BookingResponseDto(
                newBooking.Id,
                newBooking.PropertyId,
                newBooking.GuestId,
                newBooking.Dates.Start,
                newBooking.Dates.End,
                newBooking.TotalPrice,
                newBooking.Status.ToString()
            );
        }
    }
}
