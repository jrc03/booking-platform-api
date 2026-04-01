using Application.Features.Bookings.DTOs;
using MediatR;

namespace Application.Features.Bookings.Queries.GetBookingsByGuest
{
    public record GetBookingsByGuestQuery(Guid GuestId) : IRequest<IEnumerable<BookingResponseDto>>;
}