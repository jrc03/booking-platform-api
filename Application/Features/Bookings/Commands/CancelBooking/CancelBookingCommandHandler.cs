using Application.Features.Bookings.Events;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.CancelBooking
{
    public class CancelBookingCommandHandler : IRequestHandler<CancelBookingCommand, bool>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPublisher _publisher;

        public CancelBookingCommandHandler(IGenericRepository<Booking> bookingRepository, IUnitOfWork unitOfWork, IPublisher publisher)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
        }

        public async Task<bool> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId) ?? throw new Exception($"Booking with ID {request.BookingId} not found.");

            if (booking.GuestId != request.GuestId)
                throw new UnauthorizedAccessException("You are not authorized to cancel this booking.");

            booking.Cancel();

            _bookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new BookingCancelledEvent(
                booking.Id,
                booking.PropertyId,
                booking.GuestId
             ), cancellationToken);

            return true;
        }
    }
}
