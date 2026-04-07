using Application.Features.Bookings.Events;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Auth;

namespace Application.Features.Bookings.Commands.CompleteBooking
{
    public class CompleteBookingCommandHandler : IRequestHandler<CompleteBookingCommand, bool>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisher _publisher;
        private readonly ICurrentUserService _currentUserService;

        public CompleteBookingCommandHandler(IGenericRepository<Booking> bookingRepository, IUnitOfWork unitOfWork, IPublisher publisher, ICurrentUserService currentUserService)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId) ?? throw new Exception($"Booking with ID {request.BookingId} not found.");

            var guestId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Invalid user.");

            if (DateTime.UtcNow.Date <= booking.Dates.End.Date)
                throw new InvalidOperationException("Booking can only be completed after the checkout date.");
            if (booking.Status != Domain.Enums.BookingStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed bookings can be completed.");
            if (booking.GuestId != guestId)
                throw new UnauthorizedAccessException("You are not authorized to complete this booking.");

            booking.Complete();

            _bookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new BookingCompletedEvent(
                booking.Id,
                booking.PropertyId,
                booking.GuestId
            ), cancellationToken);

            return true;
        }
    }
}
