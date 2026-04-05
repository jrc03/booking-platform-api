using Application.Features.Bookings.DTOs;
using MediatR;
using System.Collections.Generic;

namespace Application.Features.Bookings.Queries.GetAllBookings;

public record GetAllBookingsQuery() : IRequest<IEnumerable<BookingResponseDto>>;
