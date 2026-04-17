using Application.Features.Properties.DTOs;
using Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Properties.Queries.GetUnavailableDates
{
    public class GetUnavailableDatesQueryHandler : IRequestHandler<GetUnavailableDatesQuery, IEnumerable<UnavailableDateRangeDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IBookingRepository _bookingRepository;

        public GetUnavailableDatesQueryHandler(IPropertyRepository propertyRepository, IBookingRepository bookingRepository)
        {
            _propertyRepository = propertyRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<UnavailableDateRangeDto>> Handle(GetUnavailableDatesQuery request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.PropertyId) ?? throw new Exception("Property not found.");
            
            var unavailableDates = new List<UnavailableDateRangeDto>();

            // Adds host manually blocked dates
            foreach (var blocked in property.BlockedDates)
            {
                unavailableDates.Add(new UnavailableDateRangeDto
                {
                    StartDate = blocked.Start,
                    EndDate = blocked.End
                });
            }

            // Adds confirmed bookings dates
            var bookings = await _bookingRepository.GetBookingsByPropertyAsync(request.PropertyId);
            foreach (var booking in bookings)
            {
                unavailableDates.Add(new UnavailableDateRangeDto
                {
                    StartDate = booking.Dates.Start,
                    EndDate = booking.Dates.End
                });
            }

            return unavailableDates.OrderBy(d => d.StartDate);
        }
    }
}