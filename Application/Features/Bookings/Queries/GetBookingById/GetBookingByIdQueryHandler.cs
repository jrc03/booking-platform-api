using Application.Features.Bookings.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;


namespace Application.Features.Bookings.Queries.GetBookingById
{
    public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingResponseDto>
    {
        private readonly IGenericRepository<Booking> _bookingRepository;

        public GetBookingByIdQueryHandler(IGenericRepository<Booking> bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponseDto> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            // TODO: Usar custom Exceptions (ej. NotFoundException) en versiones avanzadas
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId) ?? throw new Exception($"Booking with ID {request.BookingId} not found");
            
            return new BookingResponseDto(
                booking.Id,
                booking.PropertyId,
                booking.GuestId,
                booking.Dates.Start,
                booking.Dates.End,
                booking.TotalPrice,
                booking.Status.ToString()
            );
        }
    }
}