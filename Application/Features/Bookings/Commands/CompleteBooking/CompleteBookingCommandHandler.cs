using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Commands.CompleteBooking
{
    public class CompleteBookingCommandHandler : IRequestHandler<CompleteBookingCommand, bool>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteBookingCommandHandler(IGenericRepository<Booking> bookingRepository, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId) ?? throw new Exception($"Booking with ID {request.BookingId} not found.");

            if (DateTime.UtcNow.Date <= booking.Dates.End.Date)
                throw new InvalidOperationException("Booking can only be completed after the checkout date.");
            if (booking.Status != Domain.Enums.BookingStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed bookings can be completed.");
            if (booking.GuestId != request.GuestId)
                throw new UnauthorizedAccessException("You are not authorized to complete this booking.");

            booking.Complete();

            _bookingRepository.Update(booking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}