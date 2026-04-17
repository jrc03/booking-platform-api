using Application.Features.Properties.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Features.Properties.Queries.GetUnavailableDates
{
    public class GetUnavailableDatesQuery : IRequest<IEnumerable<UnavailableDateRangeDto>>
    {
        public Guid PropertyId { get; set; }

        public GetUnavailableDatesQuery(Guid propertyId)
        {
            PropertyId = propertyId;
        }
    }
}