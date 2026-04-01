using Application.Features.Bookings.DTOs;
using MediatR;
using System;

namespace Application.Features.Bookings.Queries.GetBookingById
{
    public record GetBookingByIdQuery(Guid BookingId) : IRequest<BookingResponseDto>;
}