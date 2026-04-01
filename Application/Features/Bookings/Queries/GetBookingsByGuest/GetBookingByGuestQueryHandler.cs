using Application.Features.Bookings.DTOs;
using MediatR;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Bookings.Queries.GetBookingsByGuest
{
    public class GetBookingByGuestHandler : IRequestHandler<GetBookingsByGuestQuery, IEnumerable<BookingResponseDto>>
    {
        private readonly IBookingRepository _bookingRepository;

        public GetBookingByGuestHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<BookingResponseDto>> Handle(GetBookingsByGuestQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _bookingRepository.GetBookingsByGuestAsync(request.GuestId);

            return bookings.Select(b => new BookingResponseDto(
                b.Id, b.PropertyId, b.GuestId, b.Dates.Start, b.Dates.End, b.TotalPrice, b.Status.ToString()
            ));
        }
    }
}