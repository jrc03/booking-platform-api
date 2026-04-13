using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Properties.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Queries.GetPropertyById
{
    public class GetPropertyByIdQueryHandle : IRequestHandler<GetPropertyByIdQuery, PropertyResponseDto>
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertyByIdQueryHandle(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<PropertyResponseDto> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.Id) ?? throw new Exception("Property not found");

            return new PropertyResponseDto(
                property.Id,
                property.Title,
                property.Description,
                property.Location,
                property.Capacity,
                property.PricePerNight,
                property.ImageUrls
            );
        }
    }
}