using Application.Features.Bookings.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetAllBookings;

public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, IEnumerable<BookingResponseDto>>
{
    private readonly IBookingRepository _bookingRepository;

    public GetAllBookingsQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<IEnumerable<BookingResponseDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var bookings = await _bookingRepository.GetAllAsync();

        return bookings.Select(b => new BookingResponseDto(
            b.Id,
            b.PropertyId,
            b.GuestId,
            b.Dates.Start,
            b.Dates.End,
            b.TotalPrice,
            b.Status.ToString()
        ));
    }
}
