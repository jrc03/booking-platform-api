using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

namespace Application.Features.Bookings.Commands.CancelBooking
{
    public record CancelBookingCommand(Guid BookingId) : IRequest<bool>;
}