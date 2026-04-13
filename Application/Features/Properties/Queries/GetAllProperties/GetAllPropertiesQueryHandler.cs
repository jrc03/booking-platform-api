using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Properties.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Queries.GetAllProperties
{
    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, IEnumerable<PropertyResponseDto>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetAllPropertiesQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<IEnumerable<PropertyResponseDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await _propertyRepository.GetAllAsync();

            return properties.Select(p => new PropertyResponseDto(
                p.Id,
                p.Title,
                p.Description,
                p.Location,
                p.Capacity,
                p.PricePerNight,
                p.ImageUrls
            )).ToList();
        }
    }
}