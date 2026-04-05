using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Properties.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Queries.SearchProperties
{
    public class SearchPropertyQueryHandler : IRequestHandler<SearchPropertyQuery, IEnumerable<PropertyResponseDto>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public SearchPropertyQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IEnumerable<PropertyResponseDto>> Handle(SearchPropertyQuery request, CancellationToken cancellationToken)
        {
            var properties = await _propertyRepository.SearchAsync(
                request.Location,
                request.StartDate,
                request.EndDate,
                request.MinCapacity,
                request.MaxPrice,
                request.PageNumber,
                request.PageSize
            );

            return properties.Select(p => new PropertyResponseDto(
                p.Id,
                p.Title,
                p.Description,
                p.Location,
                p.Capacity,
                p.PricePerNight
            ));
        }
    }
}