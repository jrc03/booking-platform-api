using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Bookings.DTOs;
using MediatR;

namespace Application.Features.Bookings.Commands.CreateBooking
{
    public record CreateBookingCommand(
    Guid PropertyId,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<BookingResponseDto>;
}