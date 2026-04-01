using MediatR;
using System;

namespace Application.Features.Bookings.Commands.CompleteBooking
{
    public record CompleteBookingCommand(Guid BookingId, Guid GuestId) : IRequest<bool>;
}