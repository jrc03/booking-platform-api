using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Bookings.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponseDto>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingCommandHandler(IBookingRepository bookingRepository, IPropertyRepository propertyRepository, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingResponseDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId) ?? throw new Exception("Property not found");

            if (property.HostId == request.GuestId)
                throw new InvalidOperationException("Cannot book your own property");

            var requestedDates = new DateRange(request.StartDate, request.EndDate);
            property.BlockDateRange(requestedDates);



            var newBooking = Booking.Create(
                request.PropertyId,
                request.GuestId,
                requestedDates,
                property.PricePerNight
            );

            _bookingRepository.Add(newBooking);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookingResponseDto(
                newBooking.Id,
                newBooking.PropertyId,
                newBooking.GuestId,
                newBooking.Dates.Start,
                newBooking.Dates.End,
                newBooking.TotalPrice,
                newBooking.Status.ToString()
            );
        }
    }
}