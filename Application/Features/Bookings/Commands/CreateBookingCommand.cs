using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Bookings.DTOs;
using MediatR;

namespace Application.Features.Bookings.Commands
{
    public record CreateBookingCommand(
    Guid PropertyId,
    Guid GuestId,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<BookingResponseDto>;
}